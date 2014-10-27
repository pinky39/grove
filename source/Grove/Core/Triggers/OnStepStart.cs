namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class OnStepStart : Trigger, IReceive<StepStartedEvent>, IReceive<ControllerChangedEvent>
  {
    private readonly bool _activeTurn;
    private readonly bool _onlyOnce;
    private readonly bool _passiveTurn;
    private readonly Step _step;
    private readonly Trackable<bool> _wasTriggered = new Trackable<bool>();

    private OnStepStart() {}

    public OnStepStart(Step step, bool activeTurn = true, bool passiveTurn = false, bool onlyOnce = false)
    {
      _activeTurn = activeTurn;
      _step = step;
      _passiveTurn = passiveTurn;
      _onlyOnce = onlyOnce;
    }

    public void Receive(ControllerChangedEvent message)
    {
      if (_onlyOnce && message.Card == Ability.OwningCard)
      {
        _wasTriggered.Value = false;
      }
    }

    public void Receive(StepStartedEvent message)
    {
      if (message.Step != _step)
        return;

      if (_activeTurn && Controller.IsActive || _passiveTurn && !Controller.IsActive)
      {
        Set();
        _wasTriggered.Value = true;
      }
    }

    protected override void OnActivate()
    {
      _wasTriggered.Value = false;
    }

    protected override void Initialize()
    {
      _wasTriggered.Initialize(ChangeTracker);
    }
  }
}