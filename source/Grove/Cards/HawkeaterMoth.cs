namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Dsl;

  public class HawkeaterMoth : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hawkeater Moth")
        .ManaCost("{3}{G}")
        .Type("Creature Insect")
        .Text("{Flying}{EOL}Hawkeater Moth cannot be the target of spells or abilities.")
        .FlavorText("Each day at dusk the birds above the canopy grow silent.")
        .Power(1)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Flying,
          Static.Shroud
        );
    }
  }
}