namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class OnAttack : Trigger, IReceive<AttackerJoinedCombatEvent>
  {
    private readonly bool _onlyWhenDeclared;
    private readonly bool _triggerForEveryCreature;

    private OnAttack()
    {
    }

    public OnAttack(bool triggerForEveryCreature = false, bool onlyWhenDeclared = true)
    {
      _triggerForEveryCreature = triggerForEveryCreature;
      _onlyWhenDeclared = onlyWhenDeclared;
    }

    public void Receive(AttackerJoinedCombatEvent message)
    {
      if (!message.WasDeclared && _onlyWhenDeclared)
        return;

      if (!_triggerForEveryCreature && message.Attacker.Card != Ability.OwningCard)
        return;

      Set(message);
    }
  }
}