namespace Grove.Gameplay.Triggers
{
  using System;
  using System.Linq;
  using Infrastructure;
  using Messages;

  [Serializable]
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