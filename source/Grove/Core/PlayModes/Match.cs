namespace Grove
{
  using System;
  using System.Threading.Tasks;
  using System.Windows;
  using Grove.Infrastructure;
  using Grove.UserInterface;

  public class Match
  {
    private readonly MatchParameters _p;
    private int? _looser;
    private bool _playerLeftMatch;
    private bool _rematch;
    private bool _skipNextScoreUpdate;

    public Match(MatchParameters p)
    {
      _p = p;
      Application.Current.Exit += delegate { Stop(); };
    }

    public bool IsTournament
    {
      get { return _p.IsTournament; }
    }

    public bool WasStopped
    {
      get { return Game != null && Game.WasStopped; }
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

    public bool InProgress
    {
      get { return Game != null && !IsFinished; }
    }

    public string Description
    {
      get
      {
        return String.Format("{0} vs {1} - {2}. turn", Game.Players.Player1.Name, Game.Players.Player2.Name,
          Game.Turn.TurnCount);
      }
    }

    public void Start()
    {
      Game game;

      if (_p.IsSavedMatch)
      {
        game = new Game(GameParameters.Load(
          player1Controller: PlayerType.Human,
          player2Controller: PlayerType.Machine,
          savedGame: _p.SavedMatch.SavedGame,
          looser: _p.SavedMatch.Looser));

        Player1WinCount = _p.SavedMatch.Player1WinCount;
        Player2WinCount = _p.SavedMatch.Player2WinCount;
        _looser = _p.SavedMatch.Looser;

        if (game.IsFinished)
        {
          // if the game was saved when it was already finished
          // do not update the score again
          _skipNextScoreUpdate = true;
        }
      }
      else
      {
        game = new Game(GameParameters.Default(
          _p.Player1, _p.Player2));
      }

      var shouldPlayAnotherGame = RunGame(game);

      while (shouldPlayAnotherGame || _rematch)
      {
        if (_rematch)
        {
          Player1WinCount = 0;
          Player2WinCount = 0;
          _rematch = false;
          _looser = null;
        }

        game = new Game(GameParameters.Default(
          _p.Player1, _p.Player2));

        shouldPlayAnotherGame = RunGame(game);
      }
    }

    public void Rematch()
    {
      Stop();
      _rematch = true;
    }

    public void Stop()
    {
      _rematch = false;

      if (Game != null)
      {
        Game.Stop();
      }

      Ui.Shell.CloseAllDialogs();
    }

    public SavedMatch Save()
    {
      var savedMatch = new SavedMatch
        {
          Player1WinCount = Player1WinCount,
          Player2WinCount = Player2WinCount,
          SavedGame = Game.Save(),
          Looser = _looser
        };

      return savedMatch;
    }

    private void DisplayGameResults()
    {
      var viewModel = Ui.Dialogs.GameResults.Create();
      Ui.Shell.ShowModalDialog(viewModel);
      _playerLeftMatch = viewModel.PlayerLeftMatch;
    }

    private void DisplayMatchResults()
    {
      var viewModel = Ui.Dialogs.MatchResults.Create(canRematch: !IsTournament);
      Ui.Shell.ShowModalDialog(viewModel);
      _rematch = viewModel.ShouldRematch;
    }

    private bool RunGame(Game game)
    {
      Game = game;

      var playScreen = Ui.Dialogs.PlayScreen.Create();
      Ui.Shell.ChangeScreen(playScreen);

      var blocker = new ThreadBlocker();

      AggregateException exception = null;

      blocker.BlockUntilCompleted(() => Task.Factory
        .StartNew(() => Game.Start(looser: Looser), TaskCreationOptions.LongRunning)
        .ContinueWith(t => { exception = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted)
        .ContinueWith(t => blocker.Completed()));

      if (exception != null)
        throw new AggregateException(exception.InnerExceptions);

      return ProcessGameResults();
    }

    private bool ProcessGameResults()
    {
      if (Game.WasStopped)
        return false;

      var looser = UpdateScore() ?? _looser;

      if (Game.WasStopped)
        return false;

      if (IsFinished)
      {
        DisplayMatchResults();

        if (Game.WasStopped)
          return false;

        if (_rematch && !_playerLeftMatch)
        {
          return false;
        }

        return false;
      }

      DisplayGameResults();

      if (Game.WasStopped)
        return false;

      if (_playerLeftMatch)
      {
        Player2WinCount = 2;
        return false;
      }

      _looser = looser;
      return true;
    }

    private int? UpdateScore()
    {
      if (Game.Players.BothHaveLost)
        return null;

      if (Game.Players.Player1.HasLost)
      {
        if (!_skipNextScoreUpdate)
          Player2WinCount++;

        _skipNextScoreUpdate = false;
        return 0;
      }

      if (!_skipNextScoreUpdate)
        Player1WinCount++;

      _skipNextScoreUpdate = false;
      return 1;
    }
  }
}