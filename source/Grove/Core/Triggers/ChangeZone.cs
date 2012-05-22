namespace Grove.Core.Triggers
{
  using Infrastructure;
  using Messages;
  using Zones;

  public class ChangeZone : Trigger, IReceive<CardChangedZone>
  {
    public Zone From { get; set; }
    public Zone To { get; set; }

    public void Receive(CardChangedZone message)
    {
      if (Ability.OwningCard != message.Card)
        return;

      // do not trigger first time
      if (message.From == Zone.Undefined)
        return;

      if (From == Zone.Undefined && To == message.To)
        Set();
      else if (From == message.From && To == message.To)
        Set();
      else if (From == message.From && To == Zone.Undefined)
        Set();
    }      
  }
}