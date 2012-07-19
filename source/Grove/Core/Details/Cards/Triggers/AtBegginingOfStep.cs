namespace Grove.Core.Details.Cards.Triggers
{
  using Infrastructure;
  using Messages;

  public class AtBegginingOfStep : Trigger, IReceive<StepStarted>, IReceive<CardChangedZone>
  {
    public bool ActiveTurn = true;

    public bool OnlyOnceWhenInPlay;
    public bool PassiveTurn;
    public Step Step { get; set; }

    public void Receive(CardChangedZone message)
    {
      if (OnlyOnceWhenInPlay && message.Card == Ability.OwningCard && message.ToBattlefield)
      {
        CanTrigger = true;
      }
    }

    public void Receive(StepStarted message)
    {
      if (message.Step != Step)
        return;

      if (ActiveTurn && Controller.IsActive || PassiveTurn && !Controller.IsActive)
      {
        Set();

        if (OnlyOnceWhenInPlay)
          CanTrigger = false;
      }
    }
  }
}