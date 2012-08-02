namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Clear : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Clear")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Destroy target enchantment.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Effect<DestroyTargetPermanent>()
        .Category(EffectCategories.Destruction)
        .Timing(Timings.TargetRemovalInstant())
        .Cycling("{2}")
        .Targets(
          selectorAi: TargetSelectorAi.OrderByDescendingScore(),
          effectValidator:
            C.Validator(Validators.Permanent(card => card.Is().Enchantment)));
    }
  }
}