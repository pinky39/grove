﻿namespace Grove.Gameplay.Card.Triggers
{
  using System.Linq;
  using Grove.Infrastructure;
  using Messages;

  public class OnBeingTargetedBySpell : Trigger, IReceive<PlayerHasCastASpell>
  {
    public void Receive(PlayerHasCastASpell message)
    {
      if (message.Targets.Any(target => target == Ability.OwningCard))
      {
        Set(message);
      }
    }
  }
}