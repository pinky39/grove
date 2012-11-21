namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Clear : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Clear")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Destroy target enchantment.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Effect<DestroyTargetPermanents>()
        .Category(EffectCategories.Destruction)
        .Timing(Timings.InstantRemovalTarget())
        .Cycling("{2}")
        .Targets(
          selectorAi: TargetSelectorAi.OrderByDescendingScore(),
          effectValidator:
            TargetValidator(
              TargetIs.Card(card => card.Is().Enchantment),
              ZoneIs.Battlefield()));
    }
  }
}