namespace Grove.Core.Cards.Triggers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

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