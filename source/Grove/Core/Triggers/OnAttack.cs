namespace Grove.Core.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnAttack : Trigger, IReceive<AttackerDeclared>
  {
    public void Receive(AttackerDeclared message)
    {
      if (message.Attacker.Card == Ability.OwningCard)
      {
        Set();
      }
    }
  }
}