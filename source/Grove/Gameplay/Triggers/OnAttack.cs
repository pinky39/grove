namespace Grove.Gameplay.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  [Serializable]
  public class OnAttack : Trigger, IReceive<AttackerJoinedCombat>
  {
    public void Receive(AttackerJoinedCombat message)
    {
      if (message.WasDeclared && message.Attacker.Card == Ability.OwningCard)
      {
        Set();
      }
    }
  }
}