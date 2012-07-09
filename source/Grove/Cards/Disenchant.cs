namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class Disenchant : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Disenchant")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Destroy target artifact or enchantment.")
        .FlavorText("'Let Phyrexia breed evil in the darkness; my holy light will reveal its taint.{EOL}—Serra")
        .Effect<DestroyTargetPermanent>()
        .Category(EffectCategories.Destruction)
        .Timing(Timings.TargetRemovalInstant())
        .Targets(
          filter: TargetFilters.PermanentsByDescendingScore(),
          selectors:
            C.Selector(Selectors.Permanent(card => card.Is().Artifact || card.Is().Enchantment)));
    }
  }
}