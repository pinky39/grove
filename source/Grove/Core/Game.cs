namespace Grove.Core
{
  using System;
  using System.Linq;
  using Ai;
  using Controllers;
  using Infrastructure;
  using Zones;

  [Copyable]
  public class Game
  {
    private readonly StateMachine _stateMachine;
    private readonly TurnInfo _turnInfo;
    private readonly Search _search;
    private readonly Trackable<bool> _wasStopped;

    private Game() {}

    public Game(
      Combat combat,
      Players players,
      Publisher publisher,
      Stack stack,
      ChangeTracker changeTracker,
      StateMachine stateMachine,
      Decisions decisions,
      TurnInfo turnInfo,
      Search search)
    {
      Combat = combat;
      Players = players;
      Publisher = publisher;
      Stack = stack;
      ChangeTracker = changeTracker;
      Decisions = decisions;

      _stateMachine = stateMachine;
      _turnInfo = turnInfo;
      _search = search;
      _wasStopped = new Trackable<bool>(ChangeTracker);
    }

    public ChangeTracker ChangeTracker { get; private set; }
    public Combat Combat { get; private set; }
    public Decisions Decisions { get; private set; }
    public bool IsFinished { get { return Players.AnyHasLost(); } }
    public Players Players { get; set; }
    public Publisher Publisher { get; private set; }
    public int Score { get { return Players.Score; } }
    public Stack Stack { get; private set; }
    public TurnInfo Turn { get { return _turnInfo; } }

    public Search Search { get { return _search; } }

    public int CalculateHash()
    {
      var calc = new HashCalculator();

      return HashCalculator.Combine(
        calc.Calculate(Players),
        calc.Calculate(Stack),
        calc.Calculate(Turn),
        calc.Calculate(Combat)
        );
    }

    public void Restore(object snaphost)
    {
      ChangeTracker.Restore((Snapshot) snaphost);
    }

    public object Save()
    {
      return ChangeTracker.CreateSnapshot();
    }

    public void Simulate()
    {
      _stateMachine.Resume(ShouldContinue);
    }

    public void Start(int numOfTurns = int.MaxValue, bool skipPreGame = false, Player looser = null)
    {
      Decisions.Initialize(this);

      _stateMachine.Start(() => ShouldContinue() &&
        numOfTurns >= Turn.TurnCount, skipPreGame, looser);
    }

    public void Stop()
    {
      _wasStopped.Value = true;
    }

    public override string ToString()
    {
      return Turn.ToString();
    }

    private bool ShouldContinue()
    {
      return !_wasStopped.Value && !IsFinished;
    }

    public interface IFactory
    {
      Game Create();
    }
  }
}