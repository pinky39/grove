namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Scrap : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Scrap")
        .ManaCost("{2}{R}")
        .Type("Instant")
        .Text("Destroy target artifact.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Effect<DestroyTargetPermanents>()
        .Category(EffectCategories.Destruction)
        .Timing(Timings.InstantRemovalTarget())
        .Cycling("{2}")
        .Targets(
          TargetSelectorAi.OrderByDescendingScore(), 
          Target(Validators.Card(card => card.Is().Artifact), Zones.Battlefield()));
    }
  }
}