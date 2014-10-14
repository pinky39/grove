namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class WalkingCorpse : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Walking Corpse")
        .ManaCost("{1}{B}")
        .Type("Creature - Zombie")
        .FlavorText("\"Feeding a normal army is a problem of logistics. With zombies, it is an asset. Feeding is why they fight. Feeding is why they are feared.\"{EOL}—Jadar, ghoulcaller of Nephalia")
        .Power(2)
        .Toughness(2);
    }
  }
}
