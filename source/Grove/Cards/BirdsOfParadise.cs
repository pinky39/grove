namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Mana;

  public class BirdsOfParadise : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Birds of Paradise")
        .ManaCost("{G}")
        .Type("Creature - Bird")
        .Text("{Flying}{EOL}{T}: Add one mana of any color to your mana pool.")
        .FlavorText("The gods used their feathers to paint all the colors of the world.")
        .Power(0)
        .Toughness(1)
        .StaticAbilities(Static.Flying)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add one mana of any color to your mana pool.";
            p.ManaAmount(ManaAmount.Any);            
          });
    }
  }
}