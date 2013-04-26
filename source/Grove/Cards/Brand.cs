namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Brand : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Brand")
        .ManaCost("{R}")
        .Type("Instant")
        .Text(
          "Gain control of all permanents you own. (This effect lasts indefinitely.){EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .FlavorText("By this glyph I affirm your role.")
        .Cast(p =>
          {
            p.Effect = () => new GainControlOfOwnedPermanents();
            p.TimingRule(new EndOfTurn());
            p.TimingRule(new OpponentHasPermanents(c => c.Owner != c.Controller));
          });
    }
  }
}