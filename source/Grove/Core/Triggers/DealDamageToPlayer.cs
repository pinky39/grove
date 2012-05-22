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

  public class DealDamageToPlayer : Trigger, IReceive<DamageHasBeenDealt>
  {
    public bool CombatOnly { get; set; }
    public Func<Player, TriggeredAbility, bool> IsValidPlayer { get; set; }

    public void Receive(DamageHasBeenDealt message)
    {
      if (!(message.Receiver is Player))
        return;

      if (CombatOnly && !message.IsCombat)
        return;

      var triggerCard = GetTriggerSource();

      if (message.Dealer == triggerCard && IsValidPlayer((Player) message.Receiver, Ability))
        Set();
    }

    public void ToAny()
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
      if (triggerCard.Is().Enchantment || triggerCard.Is().Equipment)
      {
        if (!triggerCard.IsAttached)
          return triggerCard;

        triggerCard = triggerCard.AttachedTo;
      }

      return triggerCard;
    }
  }
}