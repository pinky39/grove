namespace Grove.Gameplay.States
{
  using Infrastructure;

  [Copyable]
  public class TurnInfo : IHashable
  {
    private readonly Trackable<State> _state = new Trackable<State>();
    private readonly Trackable<int> _stateCount = new Trackable<int>();
    private readonly Trackable<Step> _step = new Trackable<Step>();
    private readonly Trackable<int> _stepCount = new Trackable<int>();
    private readonly Trackable<int> _turnCount = new Trackable<int>(1);

    public State State
    {
      get { return _state.Value; }
      set
      {
        _state.Value = value;
        StateCount++;
      }
    }

    public Step Step
    {
      get { return _step.Value; }
      set
      {
        _step.Value = value;
        StepCount++;
        Log.Debug("Step: {0}", value);
      }
    }

    public int StepCount { get { return _stepCount.Value; } private set { _stepCount.Value = value; } }
    public int StateCount { get { return _stateCount.Value; } private set { _stateCount.Value = value; } }

    public int TurnCount
    {
      get { return _turnCount.Value; }
      set
      {
        _turnCount.Value = value;
        Log.Debug("Turn: {0}", value);
      }
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        Step.GetHashCode(),
        TurnCount,
        State.GetHashCode()
        );
    }

    public int GetStepCountAtNextTurnCleanup()
    {
      var stepsUntillEot = ((int) Step.CleanUp) - (int) Step;
      return StepCount + stepsUntillEot + (int) Step.CleanUp;
    }

    public void Initialize(Game game)
    {
      _step.Initialize(game.ChangeTracker);
      _turnCount.Initialize(game.ChangeTracker);
      _stepCount.Initialize(game.ChangeTracker);
      _state.Initialize(game.ChangeTracker);
      _stateCount.Initialize(game.ChangeTracker);
    }

    public override string ToString()
    {
      return string.Format("Turn: {0}, Step: {1}", TurnCount, Step);
    }
  }
}