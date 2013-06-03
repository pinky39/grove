namespace Grove.Gameplay
{
  using System.Threading.Tasks;
  using System.Windows;
  using Infrastructure;
  using UserInterface;
  using UserInterface.Shell;

  public class Match
  {
    private readonly Game.IFactory _gameFactory;
    private readonly MatchParameters _p;
    private readonly IShell _shell;
    private readonly ViewModelFactories _viewModels;
    private int? _looser;
    private bool _playerLeftMatch;
    private bool _rematch = true;

    public Match(MatchParameters p, IShell shell, ViewModelFactories viewModels, Game.IFactory gameFactory)
    {
      _p = p;
      _shell = shell;
      _viewModels = viewModels;
      _gameFactory = gameFactory;

      Application.Current.Exit += delegate { Stop(); };
    }

    public bool IsTournament { get { return _p.IsTournament; } }
    public bool WasStopped { get { return Game != null && Game.WasStopped; } }

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
    public bool Rematch { get; private set; }

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

    private void DisplayGameResults()
    {
      var viewModel = _viewModels.GameResults.Create();
      _shell.ShowModalDialog(viewModel);
      _playerLeftMatch = viewModel.PlayerLeftMatch;
    }

    private void DisplayMatchResults()
    {
      var viewModel = _viewModels.MatchResults.Create(canRematch: !IsTournament);
      _shell.ShowModalDialog(viewModel);
      _rematch = viewModel.ShouldRematch;
    }

    public void Run()
    {
      Game = _gameFactory.Create(GameParameters.StandardGame(
        _p.Player1, _p.Player2));

      var playScreen = _viewModels.PlayScreen.Create();
      _shell.ChangeScreen(playScreen);

      var blocker = new ThreadBlocker();

      blocker.BlockUntilCompleted(() => Task.Factory.StartNew(() =>
        {
          Game.Start(looser: Looser);
          blocker.Completed();
        }, TaskCreationOptions.LongRunning));

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
          Rematch = true;
          return;
        }

        return;
      }

      DisplayGameResults();

      if (Game.WasStopped)
        return;

      if (_playerLeftMatch)
      {
        Player2WinCount = 2;
        return;
      }

      Run();
    }

    private void SetLooser(int? looser)
    {
      _looser = looser;
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

    public void Stop()
    {
      if (Game != null)
      {
        Game.Stop();
      }

      _shell.CloseAllDialogs();
    }

    public interface IFactory
    {
      Match Create(MatchParameters p);
    }
  }
}