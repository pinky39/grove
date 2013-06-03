namespace Grove.Gameplay.Tournaments
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;
  using Artifical;
  using Infrastructure;
  using Persistance;
  using Sets;
  using UserInterface;
  using UserInterface.Messages;
  using UserInterface.Shell;

  public class TournamentRunner {}

  public class TournamentParameters
  {
    public string PlayerName { get; private set; }
    public int PlayersCount { get; private set; }
    public string[] BoosterPacks { get; private set; }
    public string TournamentPack { get; private set; }
    public SavedTournament SavedTournament { get; private set; }
    public bool IsSavedTournament { get { return SavedTournament != null; } }

    public static TournamentParameters Default(string playerName, int playersCount, string[] boosterPacks,
      string tournamentPack)
    {
      return new TournamentParameters
        {
          PlayerName = playerName,
          PlayersCount = playersCount,
          BoosterPacks = boosterPacks,
          TournamentPack = tournamentPack
        };
    }

    public static TournamentParameters Load(SavedTournament savedTournament)
    {
      return new TournamentParameters
        {
          SavedTournament = savedTournament
        };
    }
  }

  public class Tournament
  {
    private readonly DeckBuilder _deckBuilder;
    private readonly MatchRunner _matchRunner;

    private readonly MatchSimulator _matchSimulator;
    private readonly TournamentParameters _p;
    private readonly object _playersLock = new object();
    private readonly IShell _shell;
    private readonly ViewModelFactories _viewModels;
    private CardRatings _cardRatings;
    private List<TournamentPlayer> _players;
    private int _roundsToGo;

    public Tournament(TournamentParameters p, DeckBuilder deckBuilder, ViewModelFactories viewModels, IShell shell,
      MatchRunner matchRunner, MatchSimulator matchSimulator)
    {
      _p = p;
      _deckBuilder = deckBuilder;
      _viewModels = viewModels;
      _shell = shell;
      _matchRunner = matchRunner;
      _matchSimulator = matchSimulator;
      
      _cardRatings = LoadCardRatings(p.TournamentPack, p.BoosterPacks);
    }

    private TournamentPlayer HumanPlayer { get { return _players[0]; } }
    private IEnumerable<TournamentPlayer> NonHumanPlayers { get { return _players.Skip(1); } }
    private bool WasStopped { get { return CurrentMatch.WasStopped; } }
    private Match CurrentMatch { get { return _matchRunner.Current; } }

    public void Start()
    {
      if (_p.IsSavedTournament)
      {
        if (_p.SavedTournament.HasMatchInProgress)
        {
          


        }


      }
      else
      {
        _roundsToGo = CalculateRoundCount(_p.PlayersCount);      
        _players = CreatePlayers(_p.PlayersCount, _p.PlayerName);
        
        var generatedDeckCount = GenerateDecks();
        ShowEditDeckScreen(GenerateLibrary(), generatedDeckCount);

        RunTournament();
      }
    }

    private void RunTournament()
    {
      ShowResults();

      while (_roundsToGo > 0)
      {
        _roundsToGo--;

        PlayNextRound();

        if (WasStopped)
        {
          return;
        }

        ShowResults();
      }
    } 

    private SavedTournament Save()
    {
      TournamentPlayer[] playersCopy;

      lock (_playersLock)
      {
        playersCopy = _players
          .Select(x => x.Clone())
          .ToArray();
      }

      return new SavedTournament
        {
          PlayerName = _p.PlayerName,
          Players = playersCopy,
          RoundsToGo = _roundsToGo,
          SavedMatch = CurrentMatch.Save()
        };
    }

    private void ShowResults()
    {
      var leaderboard = _viewModels.LeaderBoard.Create(_players, _roundsToGo);
      _shell.ChangeScreen(leaderboard, blockUntilClosed: true);
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

    private void PlayNextRound()
    {
      var matches = CreateSwissPairings();

      SimulateRound(matches.Where(x => x.IsSimulated));
      PlayMatch(matches.Single(x => !x.IsSimulated));
    }

    private void LoadMatch(TournamentMatch tournamentMatch)
    {
      
    }

    private void PlayMatch(TournamentMatch tournamentMatch)
    {
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


      human.GamesWon += CurrentMatch.Player1WinCount;
      nonHuman.GamesWon += CurrentMatch.Player2WinCount;

      human.GamesLost += CurrentMatch.Player2WinCount;
      nonHuman.GamesLost += CurrentMatch.Player1WinCount;

      if (CurrentMatch.Player1WinCount > CurrentMatch.Player2WinCount)
      {
        human.WinCount++;
        nonHuman.LooseCount++;
      }
      else if (CurrentMatch.Player1WinCount < CurrentMatch.Player2WinCount)
      {
        nonHuman.WinCount++;
        human.LooseCount++;
      }
      else
      {
        human.DrawCount++;
        nonHuman.DrawCount++;
      }
    }

    private void SimulateRound(IEnumerable<TournamentMatch> simulatedMatches)
    {
      Task.Factory.StartNew(() =>
        {
          foreach (var simulatedMatch in simulatedMatches)
          {
            var result = _matchSimulator.Simulate(
              simulatedMatch.Player1.Deck,
              simulatedMatch.Player2.Deck,
              maxTurnsPerGame: 20,
              maxSearchDepth: 10,
              maxTargetsCount: 1);

            UpdatePlayersWithMatchResults(simulatedMatch, result);

            _shell.Publish(new TournamentMatchFinished {Match = simulatedMatch});

            if (WasStopped)
            {
              break;
            }
          }
        }, TaskCreationOptions.LongRunning);
    }

    private void UpdatePlayersWithMatchResults(TournamentMatch simulatedMatch, MatchSimulator.SimulationResult result)
    {
      lock (_playersLock)
      {
        simulatedMatch.Player1.GamesWon += result.Deck1WinCount;
        simulatedMatch.Player2.GamesWon += result.Deck2WinCount;

        simulatedMatch.Player1.GamesLost += result.Deck2WinCount;
        simulatedMatch.Player2.GamesLost += result.Deck1WinCount;

        if (result.Deck1WinCount > result.Deck2WinCount)
        {
          simulatedMatch.Player1.WinCount++;
          simulatedMatch.Player2.LooseCount++;
        }
        else if (result.Deck1WinCount < result.Deck2WinCount)
        {
          simulatedMatch.Player2.WinCount++;
          simulatedMatch.Player1.LooseCount++;
        }
        else
        {
          simulatedMatch.Player1.DrawCount++;
          simulatedMatch.Player2.DrawCount++;
        }
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

    private void ShowEditDeckScreen(IEnumerable<string> library, int generatedDeckCount)
    {
      var screen = _viewModels.BuildLimitedDeck.Create(library, generatedDeckCount);
      _shell.ChangeScreen(screen, blockUntilClosed: true);

      HumanPlayer.Deck = screen.Result;
    }

    private static CardRatings LoadCardRatings(string tournamentPack, string[] boosterPacks)
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

      return CardRatings.Merge(merged, MediaLibrary.GetSet(tournamentPack).Ratings);
    }

    private int GenerateDecks()
    {
      const int minNumberOfGeneratedDecks = 5;
      var limitedCode = MagicSet.GetLimitedCode(_p.TournamentPack, _p.BoosterPacks);

      var preconstructed = MediaLibrary.GetDecks(limitedCode)
        .OrderBy(x => RandomEx.Next())
        .Take(NonHumanPlayers.Count() - minNumberOfGeneratedDecks)
        .ToList();

      var nonHumanPlayers = NonHumanPlayers.ToList();
      var actualNumberOfGeneratedDecks = NonHumanPlayers.Count() - preconstructed.Count;

      for (var i = 0; i < preconstructed.Count; i++)
      {
        nonHumanPlayers[i].Deck = preconstructed[i];
      }

      Task.Factory.StartNew(() =>
        {
          for (var count = 0; count < actualNumberOfGeneratedDecks; count++)
          {
            var player = nonHumanPlayers[count + preconstructed.Count];
            var library = GenerateLibrary();
            var deck = _deckBuilder.BuildDeck(library, _cardRatings);
            player.Deck = deck;

            // write generated deck to tournament folder so it can be reused in future tournaments
            var filename = Path.Combine(MediaLibrary.TournamentFolder, Guid.NewGuid() + ".dec");
            DeckFile.Write(deck, filename);

            _shell.Publish(new DeckGenerated
              {
                Count = count + 1,
              });
          }
        });

      return actualNumberOfGeneratedDecks;
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

    private List<string> GenerateLibrary()
    {
      var library = new List<string>();

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

    public interface IFactory
    {
      Tournament Create(TournamentParameters p);
    }
  }
}