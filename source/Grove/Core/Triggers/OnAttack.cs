namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnAttack : Trigger, IReceive<AttackerJoinedCombatEvent>
  {    
    private readonly Func<Card, Trigger, bool> _triggerForCreature;

    private OnAttack()
    {
    }

    public OnAttack(Func<Card, Trigger, bool> triggerForCreature = null)
    {
      _triggerForCreature = triggerForCreature ?? ( (card, trigger) => card == trigger.Ability.OwningCard);      
    }

    public void Receive(AttackerJoinedCombatEvent message)
    {      
      if (_triggerForCreature(message.Attacker.Card, this))
        Set(message);      
    }
  }
}