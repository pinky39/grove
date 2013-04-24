namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.States;

  public class Bulwark : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Bulwark")
        .ManaCost("{3}{R}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, Bulwark deals X damage to target opponent, where X is the number of cards in your hand minus the number of cards in that player's hand.")
        .FlavorText("It will be the goblin's first bath, and its last.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, Bulwark deals X damage to target opponent, where X is the number of cards in your hand minus the number of cards in that player's hand.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new DealDamageToOpponentEqualToCardDifference();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}