namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class Bulwark : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Bulwark")
        .ManaCost("{3}{R}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, Bulwark deals X damage to target opponent, where X is the number of cards in your hand minus the number of cards in that player's hand.")
        .FlavorText("'It will be the goblin's first bath, and its last.'{EOL}—Fire Eye, viashino bey")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of your upkeep, Bulwark deals X damage to target opponent, where X is the number of cards in your hand minus the number of cards in that player's hand.",
            C.Trigger<AtBegginingOfStep>((t, _) => { t.Step = Step.Upkeep; }),
            C.Effect<DealDamageToOpponentEqualToCardDifference>(),
            triggerOnlyIfOwningCardIsInPlay: true
            )
        );
    }
  }
}