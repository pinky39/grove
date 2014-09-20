namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class BirdsOfParadise : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Birds of Paradise")
        .ManaCost("{G}")
        .Type("Creature - Bird")
        .Text("{Flying}{EOL}{T}: Add one mana of any color to your mana pool.")
        .FlavorText("The gods used their feathers to paint all the colors of the world.")
        .Power(0)
        .Toughness(1)
        .SimpleAbilities(Static.Flying)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add one mana of any color to your mana pool.";
            p.ManaAmount(Mana.Any);
          });
    }
  }
}