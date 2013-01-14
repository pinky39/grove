﻿namespace Grove.Core.Triggers
{
  using System;
  using Grove.Infrastructure;
  using Grove.Core.Messages;
  using Grove.Core.Zones;

  public class OnZoneChanged : Trigger, IReceive<ZoneChanged>
  {
    public Func<TriggeredAbility, Card, bool> Filter =
      (ability, card) => ability.OwningCard == card;

    public Zone From { get; set; }
    public Zone To { get; set; }

    public void Receive(ZoneChanged message)
    {
      if (!Filter(Ability, message.Card))
        return;

      if (message.From == Zone.None)
        return;

      if (From == Zone.None && To == message.To)
        Set(message);
      else if (From == message.From && To == message.To)
        Set(message);
      else if (From == message.From && To == Zone.None)
        Set(message);
    }
  }
}