namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

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
            p.TimingRule(new OnSecondMain());
          });
    }
  }
}