namespace Grove.Core.Cards.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnStepStart : Trigger, IOrderedReceive<StepStarted>, IReceive<ZoneChanged>,
    IReceive<ControllerChanged>
  {
    public bool ActiveTurn = true;
    public bool OnlyOnceWhenAfterItComesUnderYourControl;
    public bool PassiveTurn;
    public Step Step { get; set; }
    public int Order { get; set; }

    public void Receive(StepStarted message)
    {
      if (message.Step != Step)
        return;

      if (ActiveTurn && Controller.IsActive || PassiveTurn && !Controller.IsActive)
      {
        Set();

        if (OnlyOnceWhenAfterItComesUnderYourControl)
          CanTrigger = false;
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (OnlyOnceWhenAfterItComesUnderYourControl && message.Card == Ability.OwningCard && message.ToBattlefield)
      {
        CanTrigger = true;
      }
    }

    public void Receive(ControllerChanged message)
    {
      if (OnlyOnceWhenAfterItComesUnderYourControl && message.Card == Ability.OwningCard)
      {
        CanTrigger = true;
      }
    }
  }
}