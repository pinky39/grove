namespace Grove.Core.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnStepStart : Trigger, IOrderedReceive<StepStarted>, IReceive<ZoneChanged>,
    IReceive<ControllerChanged>
  {
    private readonly bool _activeTurn;
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
      if (message.Step != _step)
        return;

      if (_activeTurn && Controller.IsActive || _passiveTurn && !Controller.IsActive)
      {
        Set();

        if (_onlyOnceWhenAfterItComesUnderYourControl)
          CanTrigger = false;
      }
    }

    public void Receive(ControllerChanged message)
    {
      if (_onlyOnceWhenAfterItComesUnderYourControl && message.Card == Ability.OwningCard)
      {
        CanTrigger = true;
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (_onlyOnceWhenAfterItComesUnderYourControl && message.Card == Ability.OwningCard && message.ToBattlefield)
      {
        CanTrigger = true;
      }
    }
  }
}