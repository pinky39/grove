namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using AI;
  using Grove.Infrastructure;
  using Grove.UserInterface;
  using Grove.UserInterface.Messages;

  public class Tournament
  {
    private readonly TournamentParameters _p;
    private readonly object _resultsLock = new object();
    private CardRatings _cardRatings;
    private List<CardInfo> _humanLibrary;
    private List<TournamentMatch> _matches;
    private List<TournamentPlayer> _players;
    private int _roundsLeft;
    private bool _shouldStop;
    private TournamentType _type;

    public Tournament(TournamentParameters p)
    {
      _p = p;
    }

    private TournamentPlayer HumanPlayer
    {
      get { return _players.Single(x => x.IsHuman); }
    }

    private IEnumerable<TournamentPlayer> NonHumanPlayers
    {
      get { return _players.Where(x => !x.IsHuman); }
    }

    private bool MatchInProgress
    {
      get
      {
        var humanMatch = GetHumanMatch();
        return humanMatch != null && !humanMatch.IsFinished;
      }
    }

    public string Description
    {
      get
      {
        {
          return string.Format("{0} tournament, {1} players, {2} rounds left", _type, _players.Count, _roundsLeft);
        }
      }
    }

    public void Start()
    {
      if (_p.IsSavedTournament)
      {
        LoadTournament();
      }
      else
      {
        NewTournament();
      }

      RunTournament();
    }

    public SavedTournament Save()
    {
      List<TournamentMatch> matches = null;
      List<TournamentPlayer> players;

      lock (_resultsLock)
      {
        // obtain a thread safe copy of current state        
        if (_matches != null)
        {
          matches = new CopyService().CopyRoot(_matches);
          players = matches.SelectMany(x => new[] {x.Player1, x.Player2}).ToList();
        }
        else
        {
          players = new CopyService().CopyRoot(_players);
        }
      }

      return new SavedTournament
        {
          RoundsToGo = _roundsLeft,
          CurrentRoundMatches = matches,
          Players = players,
          SavedMatch = MatchInProgress ? Ui.Match.Save() : null,
          HumanLibrary = _humanLibrary,
          Type = _type
        };
    }

    public void Stop()
    {
      _shouldStop = true;
    }

    private TournamentMatch GetHumanMatch()
    {
      if (_matches == null)
        return null;

      return _matches.FirstOrDefault(x => !x.IsSimulated);
    }


    private void FinishUnfinishedMatches()
    {
      if (_matches == null) return;

      AggregateException exception = null;

      SimulateRound().ContinueWith(t => { exception = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
      FinishCurrentMatch();

      if (exception != null)
        throw new AggregateException(exception.InnerExceptions);
    }

    private void NewTournament()
    {
      _cardRatings = LoadCardRatings(_p.BoosterPacks, _p.TournamentPack);
      _roundsLeft = CalculateRoundCount(_p.PlayersCount);
      _players = CreatePlayers(_p.PlayersCount, _p.PlayerName);
      _type = _p.Type;

      switch (_type)
      {
        case TournamentType.Sealed:
          {
            CreateSealedDecks();
            break;
          }
        case TournamentType.Draft:
          {
            CreateDraftDecks();
          }
          break;
      }
    }

    private void CreateDraftDecks()
    {
      var draftScreen = Ui.Dialogs.DraftScreen.Create(_players);
      Ui.Shell.ChangeScreen(draftScreen);

      var runner = new DraftRunner(draftScreen);
      var results = runner.Run(_players.Count, _p.BoosterPacks, _cardRatings);

      if (draftScreen.PlayerLeftDraft)
      {
        Stop();
        return;
      }

      var nonHumanPlayers = NonHumanPlayers.ToList();
      var nonHumanLibraries = results.Libraries.Skip(1).ToList();
      _humanLibrary = results.Libraries[0];

      AggregateException exception = null;

      Task.Factory.StartNew(() =>
        {
          for (var count = 0; count < nonHumanPlayers.Count; count++)
          {
            var player = nonHumanPlayers[count];
            var library = nonHumanLibraries[count];
            var deck = DeckBuilder.BuildDeck(library, _cardRatings);
            player.Deck = deck;

            Ui.Publisher.Publish(new DeckGenerationStatus
              {
                PercentCompleted = (int) Math.Round((100*(count + 1.0))/nonHumanPlayers.Count)
              });

            if (_shouldStop)
            {
              break;
            }
          }
        })
        .ContinueWith(t =>
          {
            exception = t.Exception;
            Ui.Publisher.Publish(new DeckGenerationError());
          }, TaskContinuationOptions.OnlyOnFaulted);


      CreateHumanDeck();

      if (exception != null)
        throw new AggregateException(exception.InnerExceptions);
    }

    private void LoadTournament()
    {
      _roundsLeft = _p.SavedTournament.RoundsToGo;
      _players = new List<TournamentPlayer>(_p.SavedTournament.Players);
      _matches = _p.SavedTournament.CurrentRoundMatches;
      _humanLibrary = _p.SavedTournament.HumanLibrary;
      _type = _p.SavedTournament.Type;

      FinishUnfinishedMatches();
    }

    private void FinishCurrentMatch()
    {
      if (_p.SavedTournament.HasMatchInProgress)
      {
        var mp = MatchParameters.Load(_p.SavedTournament.SavedMatch, isTournament: true);


        Ui.Match = new Match(mp);
        Ui.Match.Start();

        var humanMatch = GetHumanMatch();

        if (Ui.Match.WasStopped)
        {
          _shouldStop = true;
          return;
        }

        lock (_resultsLock)
        {
          if (humanMatch.Player1.IsHuman)
          {
            humanMatch.Player1WinCount = Ui.Match.Player1WinCount;
            humanMatch.Player2WinCount = Ui.Match.Player2WinCount;
          }
          else
          {
            humanMatch.Player2WinCount = Ui.Match.Player1WinCount;
            humanMatch.Player1WinCount = Ui.Match.Player2WinCount;
          }

          UpdatePlayersWithMatchResults(humanMatch);
          humanMatch.IsFinished = true;
        }
      }
    }

    private void RunTournament()
    {
      AggregateException exception = null;

      while (!_shouldStop)
      {
        ShowResults();

        if (_roundsLeft == 0 || _shouldStop)
          return;

        _roundsLeft--;

        _matches = CreateSwissPairings();

        SimulateRound().ContinueWith(t => { exception = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
        PlayMatch();

        if (exception != null)
          throw new AggregateException(exception.InnerExceptions);
      }
    }

    private void ShowResults()
    {
      var leaderboard = Ui.Dialogs.LeaderBoard.Create(_players, _roundsLeft, _humanLibrary);
      Ui.Shell.ChangeScreen(leaderboard, blockUntilClosed: true);

      if (leaderboard.ShouldQuitTournament)
      {
        Stop();
      }
    }

    private int CalculateRoundCount(int playersCount)
    {
      if (playersCount <= 8)
        return 3;
      if (playersCount <= 16)
        return 4;
      if (playersCount <= 32)
        return 5;
      if (playersCount <= 32)
        return 5;
      if (playersCount <= 64)
        return 6;
      if (playersCount <= 128)
        return 7;
      if (playersCount <= 226)
        return 8;
      if (playersCount <= 409)
        return 9;

      return 10;
    }

    private void PlayMatch()
    {
      var tournamentMatch = _matches.Single(x => !x.IsSimulated);

      var human = tournamentMatch.HumanPlayer;
      var nonHuman = tournamentMatch.NonHumanPlayer;

      Ui.Match = new Match(MatchParameters.Default(
        player1: new PlayerParameters
          {
            Name = human.Name,
            AvatarId = human.AvatarId,
            Deck = human.Deck
          },
        player2: new PlayerParameters
          {
            Name = nonHuman.Name,
            AvatarId = nonHuman.AvatarId,
            Deck = nonHuman.Deck
          },
        isTournament: true));

      Ui.Match.Start();

      if (Ui.Match.WasStopped)
      {
        _shouldStop = true;
        return;
      }

      lock (_resultsLock)
      {
        if (tournamentMatch.Player1.IsHuman)
        {
          tournamentMatch.Player1WinCount = Ui.Match.Player1WinCount;
          tournamentMatch.Player2WinCount = Ui.Match.Player2WinCount;
        }
        else
        {
          tournamentMatch.Player2WinCount = Ui.Match.Player1WinCount;
          tournamentMatch.Player1WinCount = Ui.Match.Player2WinCount;
        }

        UpdatePlayersWithMatchResults(tournamentMatch);
        tournamentMatch.IsFinished = true;
      }
    }

    private Task SimulateRound()
    {
      return Task.Factory.StartNew(() =>
        {
          foreach (var simulatedMatch in _matches.Where(x => x.IsSimulated && !x.IsFinished))
          {
            var result = MatchSimulator.Simulate(
              simulatedMatch.Player1.Deck,
              simulatedMatch.Player2.Deck,
              maxTurnsPerGame: 20,
              maxSearchDepth: 10,
              maxTargetsCount: 1);

            lock (_resultsLock)
            {
              simulatedMatch.Player1WinCount = result.Deck1WinCount;
              simulatedMatch.Player2WinCount = result.Deck2WinCount;

              UpdatePlayersWithMatchResults(simulatedMatch);
              simulatedMatch.IsFinished = true;
            }

            Ui.Publisher.Publish(new TournamentMatchFinished {Match = simulatedMatch});

            if ((Ui.Match != null && Ui.Match.WasStopped) || _shouldStop)
            {
              _shouldStop = true;
              break;
            }
          }
        }, TaskCreationOptions.LongRunning);
    }

    private void UpdatePlayersWithMatchResults(TournamentMatch match)
    {
      match.Player1.GamesWon += match.Player1WinCount;
      match.Player2.GamesWon += match.Player2WinCount;

      match.Player1.GamesLost += match.Player2WinCount;
      match.Player2.GamesLost += match.Player1WinCount;

      if (match.Player1WinCount > match.Player2WinCount)
      {
        match.Player1.WinCount++;
        match.Player2.LooseCount++;
      }
      else if (match.Player1WinCount < match.Player2WinCount)
      {
        match.Player2.WinCount++;
        match.Player1.LooseCount++;
      }
      else
      {
        match.Player1.DrawCount++;
        match.Player2.DrawCount++;
      }
    }

    private List<TournamentMatch> CreateSwissPairings()
    {
      var pairings = _players
        .OrderByDescending(x => x.MatchPoints)
        .ThenBy(x => RandomEx.Next(0, _players.Count))
        .ToArray();

      var matches = new List<TournamentMatch>();

      for (var i = 0; i < pairings.Length; i = i + 2)
      {
        var player1 = pairings[i];
        var player2 = pairings[i + 1];

        matches.Add(new TournamentMatch(player1, player2));
      }
      return matches;
    }

    private void CreateHumanDeck()
    {
      var screen = Ui.Dialogs.BuildLimitedDeck.Create(_humanLibrary);
      Ui.Shell.ChangeScreen(screen, blockUntilClosed: true);

      if (screen.WasCanceled)
      {
        Stop();
        return;
      }

      HumanPlayer.Deck = screen.Result;
    }

    private static CardRatings LoadCardRatings(string[] boosterPacks, string tournamentPack = null)
    {
      CardRatings merged = null;

      foreach (var setName in boosterPacks)
      {
        var ratings = MediaLibrary.GetSet(setName).Ratings;
        if (merged == null)
        {
          merged = ratings;
        }
        else
        {
          merged = CardRatings.Merge(merged, ratings);
        }
      }

      if (tournamentPack == null)
        return merged;

      return CardRatings.Merge(merged, MediaLibrary.GetSet(tournamentPack).Ratings);
    }

    private void CreateSealedDecks()
    {
      AggregateException exception = null;

      Task.Factory.StartNew(() =>
        {
          const int minNumberOfGeneratedDecks = 5;
          var limitedCode = MagicSet.GetLimitedCode(_p.TournamentPack, _p.BoosterPacks);
          var pregeneratedCount = NonHumanPlayers.Count() - minNumberOfGeneratedDecks;

          var pregenerated = PregeneratedDecks
            .GetRandom(limitedCode, pregeneratedCount);

          var nonHumanPlayers = NonHumanPlayers.ToList();
          var decksToGenerate = NonHumanPlayers.Count() - pregenerated.Count;

          for (var i = 0; i < pregenerated.Count; i++)
          {
            nonHumanPlayers[i].Deck = pregenerated[i];
          }


          for (var count = 0; count < decksToGenerate; count++)
          {
            var player = nonHumanPlayers[count + pregenerated.Count];
            var library = GenerateLibrary();
            var deck = DeckBuilder.BuildDeck(library, _cardRatings);
            deck.LimitedCode = limitedCode;
            player.Deck = deck;

            // save generated deck so it can be reused in future tournaments
            PregeneratedDecks.Write(deck);

            Ui.Publisher.Publish(new DeckGenerationStatus
              {
                PercentCompleted = (int) Math.Round((100*(count + 1.0))/decksToGenerate)
              });

            if (_shouldStop)
            {
              break;
            }
          }
        })
        .ContinueWith(t =>
          {
            exception = t.Exception;
            Ui.Publisher.Publish(new DeckGenerationError());
          }, TaskContinuationOptions.OnlyOnFaulted);

      _humanLibrary = GenerateLibrary();
      CreateHumanDeck();

      if (exception != null)
        throw new AggregateException(exception.InnerExceptions);
    }

    private static List<TournamentPlayer> CreatePlayers(int playersCount, string playerName)
    {
      var players = new List<TournamentPlayer>();

      var names = NameGenerator.GenerateRandomNames(MediaLibrary.GetPlayerUnitNames(), playersCount - 1);
      players.Add(new TournamentPlayer(playerName, isHuman: true));

      for (var i = 0; i < playersCount - 1; i++)
      {
        players.Add(new TournamentPlayer(names[i], isHuman: false));
      }

      return players;
    }

    private List<CardInfo> GenerateLibrary()
    {
      var library = new List<CardInfo>();

      foreach (var setName in _p.BoosterPacks)
      {
        library.AddRange(MediaLibrary
          .GetSet(setName)
          .GenerateBoosterPack());
      }

      library.AddRange(MediaLibrary
        .GetSet(_p.TournamentPack)
        .GenerateTournamentPack());

      return library;
    }
  }
}