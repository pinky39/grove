namespace Grove
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class TurnInfo : GameObject, IHashable
  {
    private readonly Trackable<TurnEvents> _events = new Trackable<TurnEvents>();
    private readonly Trackable<int> _state = new Trackable<int>();
    private readonly Trackable<int> _stateCount = new Trackable<int>();
    private readonly Trackable<Step> _step = new Trackable<Step>();
    private readonly Trackable<int> _stepCount = new Trackable<int>();
    private readonly Trackable<int> _turnCount = new Trackable<int>(0);    

    public int State
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

        LogFile.Debug("Step: {0}", value);
      }
    }

    public TurnEvents Events { get { return _events.Value; } }
    public int StepCount { get { return _stepCount.Value; } private set { _stepCount.Value = value; } }
    public int StateCount { get { return _stateCount.Value; } private set { _stateCount.Value = value; } }

    public int TurnCount { get { return _turnCount.Value; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        Step.GetHashCode(),
        TurnCount,
        State.GetHashCode()
        );
    }

    public void NextTurn()
    {
      _turnCount.Value++;
      LogFile.Debug("Turn: {0}", _turnCount.Value);

      CreateEvents();
      Publish(new TurnStarted {TurnCount = _turnCount.Value});
    }

    private void CreateEvents()
    {
      var events = new TurnEvents();
      events.Initialize(Game);
      Subscribe(events);

      if (_events.Value != null)
      {
        Unsubscribe(_events.Value);
      }

      _events.Value = events;
    }

    public int GetStepCountAtNextTurnCleanup()
    {
      var stepsUntillEot = ((int) Step.CleanUp) - (int) Step;
      return StepCount + stepsUntillEot + (int) Step.CleanUp;
    }

    public void Initialize(Game game)
    {
      Game = game;

      _step.Initialize(ChangeTracker);
      _turnCount.Initialize(ChangeTracker);
      _stepCount.Initialize(ChangeTracker);
      _state.Initialize(ChangeTracker);
      _stateCount.Initialize(ChangeTracker);      

      _events.Initialize(ChangeTracker);

      CreateEvents();
    }

    public override string ToString()
    {
      return string.Format("Turn: {0}, Step: {1}", TurnCount, Step);
    }
  }
}