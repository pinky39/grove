namespace Grove.Triggers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnStepStart : Trigger, IOrderedReceive<StepStarted>,
    IReceive<ControllerChanged>
  {
    private readonly bool _activeTurn;
    private readonly Trackable<bool> _canTrigger = new Trackable<bool>(true);
    private readonly bool _onlyOnceAfterActivated;
    private readonly bool _passiveTurn;
    private readonly Step _step;

    private OnStepStart() {}

    public OnStepStart(Step step, bool activeTurn = true, bool onlyOnceAfterActivated = false,
      bool passiveTurn = false, int order = 0)
    {
      _activeTurn = activeTurn;
      _step = step;
      _passiveTurn = passiveTurn;
      _onlyOnceAfterActivated = onlyOnceAfterActivated;
      Order = order;
    }

    public int Order { get; private set; }

    public void Receive(StepStarted message)
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

    public void Receive(ControllerChanged message)
    {
      if (_onlyOnceAfterActivated && message.Card == Ability.OwningCard)
      {
        _canTrigger.Value = true;
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