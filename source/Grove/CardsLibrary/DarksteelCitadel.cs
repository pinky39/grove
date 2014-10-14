namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class DarksteelCitadel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Darksteel Citadel")
        .Type("Artifact Land")
        .Text(
          "Indestructible (Effects that say \"destroy\" don't destroy this land.){EOL}{T}: Add {1} to your mana pool.")
        .FlavorText("Structures built from darksteel yield to neither assault nor age.")
        .SimpleAbilities(Static.Indestructible)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {1} to your mana pool.";
            p.ManaAmount(1.Colorless());
          });
    }
  }
}