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
    private readonly Trackable<bool> _canTrigger = new Trackable<bool>();

    private OnStepStart() {}

    public OnStepStart(
      Step step, 
      bool activeTurn = true, 
      bool passiveTurn = false, 
      bool onlyOnceAfterActivated = false)
    {
      _activeTurn = activeTurn;
      _step = step;
      _passiveTurn = passiveTurn;
      _onlyOnce = onlyOnceAfterActivated;
    }

    public void Receive(ControllerChangedEvent message)
    {
      if (_onlyOnce && message.Card == Ability.OwningCard)
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
        
        if (_onlyOnce)
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