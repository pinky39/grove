namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;

  public class BirdsOfParadise : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Birds of Paradise")
        .ManaCost("{G}")
        .Type("Creature - Bird")
        .Text("{Flying}{EOL}{T}: Add one mana of any color to your mana pool.")
        .FlavorText("The gods used their feathers to paint all the colors of the world.")
        .Power(0)
        .Toughness(1)
        .Timing(Timings.SecondMain())
        .Abilities(
          Static.Flying,
          C.ManaAbility(Mana.Any, "{T}: Add one mana of any color to your mana pool.")
        );
    }
  }
}