namespace Grove.Core
{
  using System;
  using System.Threading.Tasks;
  using System.Windows;
  using Decisions;
  using Infrastructure;
  using Ui.GameResults;
  using Ui.Shell;

  public class Match
  {
    private readonly CardDatabase _cardDatabase;
    private readonly DecisionSystem _decisionSystem;
    private readonly ViewModel.IFactory _gameResultsFactory;
    private readonly Ui.MatchResults.ViewModel.IFactory _matchResultsFactory;
    private readonly Ui.PlayScreen.ViewModel.IFactory _playScreenFactory;
    private readonly Ui.StartScreen.ViewModel.IFactory _startScreenFactory;
    private readonly TaskScheduler _uiScheduler;
    private Task _backgroundTask;
    private Deck _deck1;
    private Deck _deck2;
    private int? _looser;
    private bool _playerLeftMatch;
    private bool _rematch = true;
    private IShell _shell;
    private ThreadBlocker _threadBlocker;

    public Match(
      Ui.PlayScreen.ViewModel.IFactory playScreenFactory,
      Ui.StartScreen.ViewModel.IFactory startScreenFactory,
      ViewModel.IFactory gameResultsFactory,
      Ui.MatchResults.ViewModel.IFactory matchResultsFactory,
      CardDatabase cardDatabase, DecisionSystem decisionSystem)
    {
      _playScreenFactory = playScreenFactory;
      _startScreenFactory = startScreenFactory;
      _gameResultsFactory = gameResultsFactory;
      _matchResultsFactory = matchResultsFactory;
      _cardDatabase = cardDatabase;
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
      ViewModel viewModel = _gameResultsFactory.Create();
      _shell.ShowModalDialog(viewModel);
      _playerLeftMatch = viewModel.PlayerLeftMatch;
    }

    private void DisplayMatchResults()
    {
      Ui.MatchResults.ViewModel viewModel = _matchResultsFactory.Create();
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
          Game = Game.New(_deck1.CardNames, _deck2.CardNames, _cardDatabase, _decisionSystem);

          Ui.PlayScreen.ViewModel playScreen = _playScreenFactory.Create();
          _shell.ChangeScreen(playScreen);

          Game.Start(looser: Looser);

          if (Game.WasStopped)
            return;

          int? looser = UpdateScore();
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
      Ui.StartScreen.ViewModel startScreen = _startScreenFactory.Create();
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
      bool isFinished = true;

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

    public void SetShell(Shell shell)
    {
      _shell = shell;
    }
  }
}