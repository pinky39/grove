namespace Grove.Gameplay
{
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
    private readonly ViewModelFactories _viewModels;
    private Deck _deck1;
    private Deck _deck2;
    private int? _looser;
    private string _opponentsName;
    private bool _playerLeftMatch;
    private bool _rematch = true;
    private string _yourName;

    public Match(IShell shell, ViewModelFactories viewModels, CardsDatabase cardsDatabase, DecisionSystem decisionSystem)
    {
      _shell = shell;
      _viewModels = viewModels;
      _cardsDatabase = cardsDatabase;
      _decisionSystem = decisionSystem;

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

    public void Start(string yourName, string opponentsName, Deck player1Deck, Deck player2Deck)
    {
      _deck1 = player1Deck;
      _deck2 = player2Deck;
      _yourName = yourName;
      _opponentsName = opponentsName;

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
      Game = Game.New(_yourName, _opponentsName, _deck1, _deck2,
        _cardsDatabase, _decisionSystem);

      var playScreen = _viewModels.PlayScreen.Create();
      _shell.ChangeScreen(playScreen);

      var blocker = new ThreadBlocker();

      blocker.BlockUntilCompleted(() => Task.Factory.StartNew(() =>
        {
          Game.Start(looser: Looser);
          blocker.Completed();
        }));

      if (Game.WasStopped)
        return;

      var looser = UpdateScore();
      SetLooser(looser);

      if (Game.WasStopped)
        return;

      if (IsFinished)
      {
        DisplayMatchResults();

        if (Game.WasStopped)
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

      if (Game.WasStopped)
        return;

      if (_playerLeftMatch)
      {
        ShowStartScreen();
        return;
      }

      Run();
    }

    public void Rematch()
    {
      Start(_yourName, _opponentsName, _deck1, _deck2);
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
      if (Game != null)
      {
        Game.Stop();
      }

      _shell.CloseAllDialogs();
    }
  }
}