namespace Grove.Gameplay.Tournaments
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;
  using Artifical;
  using Artifical.DraftAlgorithms;
  using Infrastructure;
  using Persistance;
  using Sets;
  using UserInterface;
  using UserInterface.Messages;
  using UserInterface.Shell;

  public class Tournament
  {
    private readonly DeckBuilder _deckBuilder;    
    private readonly MatchRunner _matchRunner;

    private readonly MatchSimulator _matchSimulator;
    private readonly IDraftingStrategies _draftStrategies;
    private readonly TournamentParameters _p;
    private readonly object _resultsLock = new object();
    private readonly IShell _shell;
    private readonly ViewModelFactories _viewModels;
    private CardRatings _cardRatings;
    private List<CardInfo> _humanLibrary;
    private List<TournamentMatch> _matches;
    private List<TournamentPlayer> _players;
    private int _roundsLeft;
    private bool _shouldStop;
    private TournamentType _type;

    public Tournament(TournamentParameters p, DeckBuilder deckBuilder, ViewModelFactories viewModels, IShell shell,
      MatchRunner matchRunner, MatchSimulator matchSimulator, IDraftingStrategies draftStrategies)
    {
      _p = p;
      _deckBuilder = deckBuilder;
      _viewModels = viewModels;
      _shell = shell;
      _matchRunner = matchRunner;
      _matchSimulator = matchSimulator;
      _draftStrategies = draftStrategies;
    }

    private TournamentPlayer HumanPlayer { get { return _players.Single(x => x.IsHuman); } }
    private IEnumerable<TournamentPlayer> NonHumanPlayers { get { return _players.Where(x => !x.IsHuman); } }
    private Match CurrentMatch { get { return _matchRunner.Current; } }

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
            var libraries = DraftPlayersLibraries();
            CreateDraftDecks(libraries);
          }
          break;
      }
    }

    private List<List<CardInfo>> DraftPlayersLibraries()
    {
      var draftScreen = _viewModels.DraftScreen.Create(_players);
      _shell.ChangeScreen(draftScreen);

      var runner = new DraftRunner(_draftStrategies, draftScreen);      
      return runner.Run(_players.Count, _p.BoosterPacks, _cardRatings)
        .Libraries;
    }

    private void CreateDraftDecks(List<List<CardInfo>> libraries)
    {
      var nonHumanPlayers = NonHumanPlayers.ToList();
      var nonHumanLibraries = libraries.Skip(1).ToList();
      _humanLibrary = libraries[0];

      AggregateException exception = null;

      Task.Factory.StartNew(() =>
        {
          for (var count = 0; count < nonHumanPlayers.Count; count++)
          {
            var player = nonHumanPlayers[count];
            var library = nonHumanLibraries[count];
            var deck = _deckBuilder.BuildDeck(library, _cardRatings);
            player.Deck = deck;

            _shell.Publish(new DeckGenerationStatus
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
            _shell.Publish(new DeckGenerationError());
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
        _matchRunner.Start(mp);

        var humanMatch = GetHumanMatch();

        if (CurrentMatch.WasStopped)
        {
          _shouldStop = true;
          return;
        }

        lock (_resultsLock)
        {
          if (humanMatch.Player1.IsHuman)
          {
            humanMatch.Player1WinCount = CurrentMatch.Player1WinCount;
            humanMatch.Player2WinCount = CurrentMatch.Player2WinCount;
          }
          else
          {
            humanMatch.Player2WinCount = CurrentMatch.Player1WinCount;
            humanMatch.Player1WinCount = CurrentMatch.Player2WinCount;
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
          SavedMatch = MatchInProgress ? CurrentMatch.Save() : null,
          HumanLibrary = _humanLibrary,
          Type = _type
        };
    }

    private void ShowResults()
    {
      var leaderboard = _viewModels.LeaderBoard.Create(_players, _roundsLeft, _humanLibrary);
      _shell.ChangeScreen(leaderboard, blockUntilClosed: true);

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

      _matchRunner.Start(
        player1: new PlayerParameters
          {
            Name = human.Name,
            Avatar = "player1.png",
            Deck = human.Deck
          },
        player2: new PlayerParameters
          {
            Name = nonHuman.Name,
            Avatar = "player2.png",
            Deck = nonHuman.Deck
          },
        isTournament: true);

      if (CurrentMatch.WasStopped)
      {
        _shouldStop = true;
        return;
      }

      lock (_resultsLock)
      {
        if (tournamentMatch.Player1.IsHuman)
        {
          tournamentMatch.Player1WinCount = CurrentMatch.Player1WinCount;
          tournamentMatch.Player2WinCount = CurrentMatch.Player2WinCount;
        }
        else
        {
          tournamentMatch.Player2WinCount = CurrentMatch.Player1WinCount;
          tournamentMatch.Player1WinCount = CurrentMatch.Player2WinCount;
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
            var result = _matchSimulator.Simulate(
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

            _shell.Publish(new TournamentMatchFinished {Match = simulatedMatch});

            if ((CurrentMatch != null && CurrentMatch.WasStopped) || _shouldStop)
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
      var screen = _viewModels.BuildLimitedDeck.Create(_humanLibrary);
      _shell.ChangeScreen(screen, blockUntilClosed: true);

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
      const int minNumberOfGeneratedDecks = 5;
      var limitedCode = MagicSet.GetLimitedCode(_p.TournamentPack, _p.BoosterPacks);

      var preconstructed = MediaLibrary.GetDecks(limitedCode)
        .OrderBy(x => RandomEx.Next())
        .Take(NonHumanPlayers.Count() - minNumberOfGeneratedDecks)
        .ToList();

      var nonHumanPlayers = NonHumanPlayers.ToList();
      var decksToGenerate = NonHumanPlayers.Count() - preconstructed.Count;

      for (var i = 0; i < preconstructed.Count; i++)
      {
        nonHumanPlayers[i].Deck = preconstructed[i];
      }

      AggregateException exception = null;

      Task.Factory.StartNew(() =>
        {
          for (var count = 0; count < decksToGenerate; count++)
          {
            var player = nonHumanPlayers[count + preconstructed.Count];
            var library = GenerateLibrary();
            var deck = _deckBuilder.BuildDeck(library, _cardRatings);
            player.Deck = deck;

            // write generated deck to tournament folder so it can be reused in future tournaments
            var filename = Path.Combine(MediaLibrary.TournamentFolder, Guid.NewGuid() + ".dec");
            DeckFile.Write(deck, filename);

            _shell.Publish(new DeckGenerationStatus
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
            _shell.Publish(new DeckGenerationError());
          }, TaskContinuationOptions.OnlyOnFaulted);

      _humanLibrary = GenerateLibrary();
      CreateHumanDeck();

      if (exception != null)
        throw new AggregateException(exception.InnerExceptions);
    }

    private static List<TournamentPlayer> CreatePlayers(int playersCount, string playerName)
    {
      var players = new List<TournamentPlayer>();

      var names = MediaLibrary.NameGenerator.GenerateNames(playersCount - 1);
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

    public void Stop()
    {
      _shouldStop = true;
    }

    public interface IFactory
    {
      Tournament Create(TournamentParameters p);
    }
  }
}