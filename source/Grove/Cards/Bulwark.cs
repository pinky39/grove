namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class Bulwark : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Bulwark")
        .ManaCost("{3}{R}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, Bulwark deals X damage to target opponent, where X is the number of cards in your hand minus the number of cards in that player's hand.")
        .FlavorText("'It will be the goblin's first bath, and its last.'{EOL}—Fire Eye, viashino bey")
        .Cast(p => p.Timing = Timings.SecondMain())                
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, Bulwark deals X damage to target opponent, where X is the number of cards in your hand minus the number of cards in that player's hand.",
            Trigger<OnStepStart>(t => { t.Step = Step.Upkeep; }),
            Effect<DealDamageToOpponentEqualToCardDifference>(),
            triggerOnlyIfOwningCardIsInPlay: true
            )
        );
    }
  }
}