namespace Grove.Gameplay.Card.Triggers
{
  using Grove.Infrastructure;
  using Messages;
  using States;

  public class OnStepStart : Trigger, IOrderedReceive<StepStarted>, IReceive<ZoneChanged>,
    IReceive<ControllerChanged>
  {
    private readonly bool _activeTurn;
    private readonly Trackable<bool> _canTrigger = new Trackable<bool>(true);
    private readonly bool _onlyOnceWhenAfterItComesUnderYourControl;
    private readonly bool _passiveTurn;
    private readonly Step _step;

    private OnStepStart() {}

    public OnStepStart(Step step, bool activeTurn = true, bool onlyOnceWhenAfterItComesUnderYourControl = false,
      bool passiveTurn = false, int order = 0)
    {
      _activeTurn = activeTurn;
      _step = step;
      _passiveTurn = passiveTurn;
      _onlyOnceWhenAfterItComesUnderYourControl = onlyOnceWhenAfterItComesUnderYourControl;
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

        if (_onlyOnceWhenAfterItComesUnderYourControl)
        {
          _canTrigger.Value = false;
        }
      }
    }

    public void Receive(ControllerChanged message)
    {
      if (_onlyOnceWhenAfterItComesUnderYourControl && message.Card == Ability.OwningCard)
      {
        _canTrigger.Value = false;
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (_onlyOnceWhenAfterItComesUnderYourControl && message.Card == Ability.OwningCard && message.ToBattlefield)
      {
        _canTrigger.Value = false;
      }
    }

    protected override void Initialize()
    {
      _canTrigger.Initialize(ChangeTracker);
    }
  }
}