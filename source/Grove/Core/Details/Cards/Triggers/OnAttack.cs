namespace Grove.Core.Details.Cards.Triggers
{
  using Infrastructure;
  using Messages;

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