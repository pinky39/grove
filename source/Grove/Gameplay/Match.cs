namespace Grove.Gameplay
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Windows;
  using Decisions;
  using Infrastructure;
  using UserInterface;
  using UserInterface.Shell;

  public class Match
  {
    private readonly CardsDatabase _cardsDatabase;
    private readonly DecisionSystem _decisionSystem;
    private readonly IShell _shell;
    private readonly TaskScheduler _uiScheduler;
    private readonly ViewModelFactories _viewModels;
    private Task _backgroundTask;
    private Deck _deck1;
    private Deck _deck2;
    private int? _looser;
    private bool _playerLeftMatch;
    private bool _rematch = true;
    private ThreadBlocker _threadBlocker;

    public Match(IShell shell, ViewModelFactories viewModels, CardsDatabase cardsDatabase, DecisionSystem decisionSystem)
    {
      _shell = shell;
      _viewModels = viewModels;
      _cardsDatabase = cardsDatabase;
      _decisionSystem = decisionSystem;
      _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

      Application.Current.Exit += delegate { ForceCurrentGameToEnd(); };
    }

    public Game Game { get; private set; }

    public bool IsFinished
    {
      get
      {
        return
          Player1WinCount == 2 ||
            Player2WinCount == 2;
      }
    }

    public int Player1WinCount { get; private set; }
    public int Player2WinCount { get; private set; }

    protected Player Looser
    {
      get
      {
        if (_looser == null)
          return null;

        return Game.Players[_looser.Value];
      }
    }

    public bool InProgress { get { return Game != null && !IsFinished; } }

    public void Start(Deck player1Deck, Deck player2Deck)
    {
      _deck1 = player1Deck;
      _deck2 = player2Deck;

      ResetResults();
      Run();
    }

    private void DisplayGameResults()
    {
      var viewModel = _viewModels.GameResults.Create();
      _shell.ShowModalDialog(viewModel);
      _playerLeftMatch = viewModel.PlayerLeftMatch;
    }

    private void DisplayMatchResults()
    {
      var viewModel = _viewModels.MatchResults.Create();
      _shell.ShowModalDialog(viewModel);
      _rematch = viewModel.ShouldRematch;
    }

    private void ResetResults()
    {
      Player1WinCount = 0;
      Player2WinCount = 0;
      _playerLeftMatch = false;
      _looser = null;
    }

    private void Run()
    {
      _backgroundTask = new Task(() =>
        {
          var yourName = "You";
          var opponentsName = MediaLibrary.NameGenerator.GenerateName();

          Game = Game.New(yourName, opponentsName, _deck1.ToList(), _deck2.ToList(),
            _cardsDatabase, _decisionSystem);

          var playScreen = _viewModels.PlayScreen.Create();
          _shell.ChangeScreen(playScreen);

          Game.Start(looser: Looser);

          if (Game.WasStopped)
            return;

          var looser = UpdateScore();
          SetLooser(looser);
        });

      _backgroundTask.ContinueWith(task =>
        {
          if (task.Exception != null)
          {
            throw new Exception("A fatal error has occured.", task.Exception);
          }

          if (HasGameBeenStopped())
            return;

          if (IsFinished)
          {
            DisplayMatchResults();

            if (HasGameBeenStopped())
              return;

            if (_rematch && !_playerLeftMatch)
            {
              Rematch();
              return;
            }

            ShowStartScreen();
            return;
          }

          DisplayGameResults();

          if (HasGameBeenStopped())
            return;

          if (_playerLeftMatch)
          {
            ShowStartScreen();
            return;
          }

          Run();
        }, _uiScheduler);


      _backgroundTask.Start(TaskScheduler.Default);
    }

    private bool HasGameBeenStopped()
    {
      if (Game.WasStopped)
      {
        if (_threadBlocker != null)
        {
          _threadBlocker.Completed();
          _backgroundTask = null;
        }
        return true;
      }

      return false;
    }

    public void Rematch()
    {
      Start(_deck1, _deck2);
    }

    private void SetLooser(int? looser)
    {
      _looser = looser;
    }

    private void ShowStartScreen()
    {
      var startScreen = _viewModels.StartScreen.Create();
      _shell.ChangeScreen(startScreen);
    }

    private int? UpdateScore()
    {
      if (Game.Players.BothHaveLost)
        return null;

      if (Game.Players.Player1.HasLost)
      {
        Player2WinCount++;
        return 0;
      }

      Player1WinCount++;
      return 1;
    }

    public void ForceCurrentGameToEnd()
    {
      var isFinished = true;

      if (Game != null)
      {
        isFinished = Game.IsFinished;
        Game.Stop();
      }

      _shell.CloseAllDialogs();

      if (!isFinished)
        WaitForGameToFinish();
    }

    private void WaitForGameToFinish()
    {
      if (_backgroundTask != null)
      {
        _threadBlocker = new ThreadBlocker();
        _threadBlocker.BlockUntilCompleted();
        _threadBlocker = null;
      }
    }
  }
}