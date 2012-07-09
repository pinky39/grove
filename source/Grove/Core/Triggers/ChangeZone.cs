namespace Grove.Core.Triggers
{
  using System;
  using Infrastructure;
  using Messages;
  using Zones;


  public class ChangeZone : Trigger, IReceive<CardChangedZone>
  {
    public Zone From { get; set; }
    public Zone To { get; set; }
    
    public Func<TriggeredAbility, Card, bool> Filter = 
      (ability, card) => ability.OwningCard == card;

    public void Receive(CardChangedZone message)
    {
      if (!Filter(Ability, message.Card))
        return;
      
      if (message.From == Zone.Undefined)
        return;

      if (From == Zone.Undefined && To == message.To)
        Set(message);
      else if (From == message.From && To == message.To)
        Set(message);
      else if (From == message.From && To == Zone.Undefined)
        Set(message);
    }      
  }
}