namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnAttack : Trigger, IReceive<AttackerJoinedCombatEvent>
  {
    private readonly bool _onlyWhenDeclared;
    private readonly Func<Card, Trigger, bool> _triggerForCreature;

    private OnAttack()
    {
    }

    public OnAttack(Func<Card, Trigger, bool> triggerForCreature = null, bool onlyWhenDeclared = true)
    {
      _triggerForCreature = triggerForCreature ?? ( (card, trigger) => card == trigger.Ability.OwningCard);
      _onlyWhenDeclared = onlyWhenDeclared;
    }

    public void Receive(AttackerJoinedCombatEvent message)
    {
      if (!message.WasDeclared && _onlyWhenDeclared)
        return;

      if (_triggerForCreature(message.Attacker.Card, this))
        Set(message);      
    }
  }
}