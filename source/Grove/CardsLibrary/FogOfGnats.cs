namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class FogOfGnats : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fog of Gnats")
        .ManaCost("{B}{B}")
        .Type("Creature Insect")
        .Text("{Flying}{EOL}{B}: Regenerate Fog of Gnats.")
        .FlavorText("You can swat a thousand gnats, but a thousand more will assail you.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Flying)
        .Regenerate(cost: Mana.Black, text: "{B}: Regenerate Fog of Gnats.");
    }
  }
}