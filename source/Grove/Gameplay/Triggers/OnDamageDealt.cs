namespace Grove.Gameplay.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public class OnDamageDealt : Trigger, IReceive<DamageHasBeenDealt>
  {
    private readonly bool _combatOnly;
    private readonly Func<Card, OnDamageDealt, Damage, bool> _creatureFilter;
    private readonly bool _onlyByTriggerSource;
    private readonly Func<Player, OnDamageDealt, Damage, bool> _playerFilter;
    private readonly bool _useAttachedToAsTriggerSource;

    private OnDamageDealt() {}

    public OnDamageDealt(bool combatOnly = false, bool useAttachedToAsTriggerSource = false,
      bool onlyByTriggerSource = true,
      Func<Card, OnDamageDealt, Damage, bool> creatureFilter = null,
      Func<Player, OnDamageDealt, Damage, bool> playerFilter = null)
    {
      _combatOnly = combatOnly;
      _useAttachedToAsTriggerSource = useAttachedToAsTriggerSource;
      _onlyByTriggerSource = onlyByTriggerSource;
      _creatureFilter = creatureFilter ?? delegate { return false; };
      _playerFilter = playerFilter ?? delegate { return false; };
    }

    public void Receive(DamageHasBeenDealt message)
    {
      if (_combatOnly && !message.Damage.IsCombat)
        return;

      var triggerCard = GetTriggerSource();

      if (
        (!_onlyByTriggerSource || (message.Damage.Source == triggerCard)) &&
          IsValid(message)
        )
        Set(message);
    }

    private bool IsValid(DamageHasBeenDealt message)
    {
      var player = message.Receiver as Player;

      if (player != null)
        return _playerFilter(player, this, message.Damage);

      var creature = message.Receiver as Card;

      if (creature != null)
        return _creatureFilter(creature, this, message.Damage);

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