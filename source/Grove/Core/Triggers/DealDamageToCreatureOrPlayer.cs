namespace Grove.Core.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public enum PlayerRelationshipToAbility
  {
    OwningCardController,
    OwningCardControllersOpponent,
    SourceCardController
  }

  public class DealDamageToCreatureOrPlayer : Trigger, IReceive<DamageHasBeenDealt>
  {
    public Func<Card, TriggeredAbility, bool> IsValidCreature = delegate { return false; };
    public Func<Player, TriggeredAbility, bool> IsValidPlayer = delegate { return false; };
    public bool CombatOnly { get; set; }
    public bool UseAttachedToAsTriggerSource { get; set; }

    public void Receive(DamageHasBeenDealt message)
    {
      if (CombatOnly && !message.Damage.IsCombat)
        return;

      var triggerCard = GetTriggerSource();

      if (message.Damage.Source == triggerCard && IsValid(message.Receiver))
        Set(message);
    }

    private bool IsValid(object damageReceiver)
    {
      var player = damageReceiver as Player;

      if (player != null)
        return IsValidPlayer(player, Ability);

      var creature = damageReceiver as Card;

      if (creature != null)
        return IsValidCreature(creature, Ability);

      return false;
    }

    public void ToAnyPlayer()
    {
      IsValidPlayer = (receiver, ability) => true;
    }

    public void ToOpponent()
    {
      IsValidPlayer = (receiver, ability) => receiver != ability.SourceCard.Controller;
    }

    public void ToYou()
    {
      IsValidPlayer = (receiver, ability) => receiver == ability.SourceCard.Controller;
    }

    private Card GetTriggerSource()
    {
      var triggerCard = Ability.OwningCard;

      // if ability card is equipment or enchantment, set triggerCard to
      // card to which ability card is attached
      if (UseAttachedToAsTriggerSource && (triggerCard.Is().Enchantment || triggerCard.Is().Equipment))
      {
        if (!triggerCard.IsAttached)
          return triggerCard;

        triggerCard = triggerCard.AttachedTo;
      }

      return triggerCard;
    }

    public void ToAnyCreature()
    {
      IsValidCreature = delegate { return true; };
    }
  }
}