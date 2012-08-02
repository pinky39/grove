namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Scrap : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Scrap")
        .ManaCost("{2}{R}")
        .Type("Instant")
        .Text("Destroy target artifact.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Effect<DestroyTargetPermanent>()
        .Category(EffectCategories.Destruction)
        .Timing(Timings.TargetRemovalInstant())
        .Cycling("{2}")
        .Targets(
          selectorAi: TargetSelectorAi.OrderByDescendingScore(),
          effectValidator:
            C.Validator(Validators.Permanent(card => card.Is().Artifact)));
    }
  }
}