namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class Expunge : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Expunge")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text(
          "Destroy target nonartifact, nonblack creature. It can't be regenerated.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Timing(Timings.InstantRemovalTarget())
        .Category(EffectCategories.Destruction)
        .Effect<DestroyTargetPermanents>()
        .Targets(
          TargetSelectorAi.Destroy(), 
          Target(
            Validators.Card(card => !card.HasColors(ManaColors.Black) && !card.Is().Artifact),
            Zones.Battlefield()))
        ;
    }
  }
}