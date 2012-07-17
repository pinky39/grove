namespace Grove.Core
{
  using Infrastructure;
  using log4net;

  [Copyable]
  public class TurnInfo : IHashable
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (TurnInfo));
    private readonly Trackable<State> _state;
    private readonly Trackable<int> _stateCount;

    private readonly Trackable<Step> _step;
    private readonly Trackable<int> _stepCount;
    private readonly Trackable<int> _turnCount;

    private TurnInfo() {}

    public TurnInfo(ChangeTracker changeTracker)
    {
      _step = new Trackable<Step>(changeTracker);
      _turnCount = new Trackable<int>(1, changeTracker);
      _stepCount = new Trackable<int>(changeTracker);
      _state = new Trackable<State>(changeTracker);
      _stateCount = new Trackable<int>(changeTracker);
    }

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
        Log.DebugFormat("Step: {0}", value);
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
        Log.DebugFormat("Turn: {0}", value);
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

    public override string ToString()
    {
      return string.Format("Turn: {0}, Step: {1}", TurnCount, Step);
    }
  }
}