namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class Hush : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hush")
        .ManaCost("{3}{G}")
        .Type("Sorcery")
        .Text("Destroy all enchantments.{EOL}{Cycling} {2} ({2}, Discard this card: Draw a card.)")
        .Timing(Timings.FirstMain())
        .Cycling("{2}")     
        .Category(EffectCategories.Destruction)
        .Effect<DestroyAllPermanents>(e => e.Filter = (card) => card.Is().Enchantment);
    }
  }
}