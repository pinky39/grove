namespace Grove.Triggers
{
  using Grove.Events;
  using Grove.Infrastructure;

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