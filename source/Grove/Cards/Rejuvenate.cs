namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Rejuvenate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rejuvenate")
        .ManaCost("{3}{G}")
        .Type("Sorcery")
        .Text("You gain 6 life.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new ControllerGainsLife(6);
            p.TimingRule(new SecondMain());
          });
    }
  }
}