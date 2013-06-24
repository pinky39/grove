namespace Grove.Gameplay
{
  using System;
  using System.Threading.Tasks;
  using System.Windows;
  using Infrastructure;
  using Misc;
  using Persistance;
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
    private bool _rematch;
    private bool _skipNextScoreUpdate;

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

    public string Description
    {
      get
      {
        return String.Format("{0} vs {1} - {2}. turn", Game.Players.Player1.Name, Game.Players.Player2.Name,
          Game.Turn.TurnCount);
      }
    }

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

    public void Start()
    {
      Game game;   

      if (_p.IsSavedMatch)
      {                
        game = _gameFactory.Create(GameParameters.Load(
          player1Controller: ControllerType.Human,
          player2Controller: ControllerType.Machine,
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
        game = _gameFactory.Create(GameParameters.Default(
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

        game = _gameFactory.Create(GameParameters.Default(
          _p.Player1, _p.Player2));

        shouldPlayAnotherGame = RunGame(game);
      }
    }

    private bool RunGame(Game game)
    {
      Game = game;

      var playScreen = _viewModels.PlayScreen.Create();
      _shell.ChangeScreen(playScreen);

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

      _shell.CloseAllDialogs();
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

    public interface IFactory
    {
      Match Create(MatchParameters p);
    }
  }
}