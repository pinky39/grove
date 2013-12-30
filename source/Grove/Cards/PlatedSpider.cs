namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class PlatedSpider : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Plated Spider")
        .ManaCost("{4}{G}")
        .Type("Creature Spider")
        .Text("{Reach}")
        .FlavorText("Most spiders wait patiently for their prey to arrive. Most spiders aren't forty feet tall.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Reach);
    }
  }
}