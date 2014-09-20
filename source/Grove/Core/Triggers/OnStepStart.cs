namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class OnStepStart : Trigger, IReceive<StepStartedEvent>, IReceive<ControllerChangedEvent>
  {
    private readonly bool _activeTurn;
    private readonly Trackable<bool> _canTrigger = new Trackable<bool>(true);
    private readonly bool _onlyOnceAfterActivated;
    private readonly bool _passiveTurn;
    private readonly Step _step;

    private OnStepStart() {}

    public OnStepStart(Step step, bool activeTurn = true, bool onlyOnceAfterActivated = false,
      bool passiveTurn = false)
    {
      _activeTurn = activeTurn;
      _step = step;
      _passiveTurn = passiveTurn;
      _onlyOnceAfterActivated = onlyOnceAfterActivated;
    }

    public void Receive(ControllerChangedEvent message)
    {
      if (_onlyOnceAfterActivated && message.Card == Ability.OwningCard)
      {
        _canTrigger.Value = true;
      }
    }

    public void Receive(StepStartedEvent message)
    {
      if (!_canTrigger)
        return;

      if (message.Step != _step)
        return;

      if (_activeTurn && Controller.IsActive || _passiveTurn && !Controller.IsActive)
      {
        Set();

        if (_onlyOnceAfterActivated)
        {
          _canTrigger.Value = false;
        }
      }
    }

    protected override void OnActivate()
    {
      _canTrigger.Value = true;
    }

    protected override void Initialize()
    {
      _canTrigger.Initialize(ChangeTracker);
    }
  }
}