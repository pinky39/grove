namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class Brand : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Brand")
        .ManaCost("{R}")
        .Type("Instant")
        .Text(
          "Gain control of all permanents you own. (This effect lasts indefinitely.){EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .FlavorText("'By this glyph I affirm your role.'{EOL}—Urza, to Karn")
        .Timing(All(
          Timings.EndOfTurn(), 
          Timings.OpponentHasPermanent(card => card.Owner != card.Controller)))
        .Effect<GainControlOfOwnedPermanents>();
    }
  }
}