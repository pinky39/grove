namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class HawkeaterMoth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hawkeater Moth")
        .ManaCost("{3}{G}")
        .Type("Creature Insect")
        .Text("{Flying}{EOL}Hawkeater Moth cannot be the target of spells or abilities.")
        .FlavorText("Each day at dusk the birds above the canopy grow silent.")
        .Power(1)
        .Toughness(2)
        .SimpleAbilities(
          Static.Flying,
          Static.Shroud
        );
    }
  }
}