namespace Grove.Core.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public class OnDamageDealt : Trigger, IReceive<DamageHasBeenDealt>
  {
    private readonly bool _combatOnly;
    private readonly Func<Card, TriggeredAbility, bool> _creatureFilter;
    private readonly Func<Player, TriggeredAbility, bool> _playerFilter;
    private readonly bool _useAttachedToAsTriggerSource;

    private OnDamageDealt() {}

    public OnDamageDealt(bool combatOnly = false, bool useAttachedToAsTriggerSource = false,
      Func<Card, TriggeredAbility, bool> creatureFilter = null, Func<Player, TriggeredAbility, bool> playerFilter = null)
    {
      _combatOnly = combatOnly;
      _useAttachedToAsTriggerSource = useAttachedToAsTriggerSource;
      _creatureFilter = creatureFilter ?? delegate { return false; };
      _playerFilter = playerFilter ?? delegate { return false; };
    }

    public void Receive(DamageHasBeenDealt message)
    {
      if (_combatOnly && !message.Damage.IsCombat)
        return;

      var triggerCard = GetTriggerSource();

      if (message.Damage.Source == triggerCard && IsValid(message.Receiver))
        Set(message);
    }

    private bool IsValid(object damageReceiver)
    {
      var player = damageReceiver as Player;

      if (player != null)
        return _playerFilter(player, Ability);

      var creature = damageReceiver as Card;

      if (creature != null)
        return _creatureFilter(creature, Ability);

      return false;
    }


    private Card GetTriggerSource()
    {
      var triggerCard = Ability.OwningCard;

      // if ability card is equipment or enchantment, set triggerCard to
      // card to which ability card is attached
      if (_useAttachedToAsTriggerSource && (triggerCard.Is().Enchantment || triggerCard.Is().Equipment))
      {
        if (!triggerCard.IsAttached)
          return triggerCard;

        triggerCard = triggerCard.AttachedTo;
      }

      return triggerCard;
    }
  }
}