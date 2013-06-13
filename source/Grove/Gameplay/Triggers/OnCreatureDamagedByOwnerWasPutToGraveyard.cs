namespace Grove.Gameplay.Triggers
{
  using Infrastructure;
  using Messages;
  using States;

  public class OnCreatureDamagedByOwnerWasPutToGraveyard : Trigger, IReceive<DamageHasBeenDealt>, 
    IReceive<StepStarted>, IReceive<ZoneChanged>
  {
    private readonly TrackableList<Card> _damaged = new TrackableList<Card>();
    
    public void Receive(DamageHasBeenDealt message)
    {
      if (message.Damage.Source == Ability.OwningCard && message.Receiver is Card)
      {
        _damaged.Add((Card)message.Receiver);
      }
    }

    public void Receive(StepStarted message)
    {
      if (message.Step == Step.EndOfTurn)
      {
        _damaged.Clear();
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (message.FromBattlefieldToGraveyard && _damaged.Contains(message.Card)) 
        Set();
    }
  }
}