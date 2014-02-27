namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class Zephid : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Zephid")
        .ManaCost("{4}{U}{U}")
        .Type("Creature Illusion")
        .Text("{Flying}{EOL}Zephid cannot be the target of spells or abilities.")
        .FlavorText("Once you've seen one, you'll understand why spells won't go near them.")
        .Power(3)
        .Toughness(4)
        .SimpleAbilities(Static.Flying, Static.Shroud);
    }
  }
}