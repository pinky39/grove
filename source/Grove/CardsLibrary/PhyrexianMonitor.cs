namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class PhyrexianMonitor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Monitor")
        .ManaCost("{3}{B}")
        .Type("Creature Skeleton")
        .Text("{B}: Regenerate Phyrexian Monitor.")
        .FlavorText(
          ":Being one would be an honor, if the word 'honor' had any meaning in Phyrexia.")
        .Power(2)
        .Toughness(2)
        .Regenerate(cost: Mana.Black, text: "{B}: Regenerate Phyrexian Monitor.");
    }
  }
}