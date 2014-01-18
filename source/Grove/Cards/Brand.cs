namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Brand : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new GainControlOfAllPermanents((c, e) => c.Owner == e.Controller);
            p.TimingRule(new OnEndOfOpponentsTurn());
            p.TimingRule(new WhenOpponentControllsPermanents(c => c.Owner != c.Controller));
          });
    }
  }
}