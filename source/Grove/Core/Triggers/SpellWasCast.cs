namespace Grove.Core.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public class SpellWasCast : Trigger, IReceive<PlayerHasCastASpell>
  {
    public Func<TriggeredAbility, Card, bool> Filter =
      delegate { return true; };
    
    public void Receive(PlayerHasCastASpell message)
    {
      if (!Filter(Ability, message.Spell))
        return;

      Set(message);
    }
  }
}