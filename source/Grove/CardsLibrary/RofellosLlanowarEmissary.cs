namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class RofellosLlanowarEmissary : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rofellos, Llanowar Emissary")
        .Type("Legendary Creature Elf Druid")
        .Text("{T}: Add {G} to your mana pool for each Forest you control.")
        .Power(2)
        .Toughness(1)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool for each Forest you control.";
            p.ManaAmount(ManaColor.Green, c => c.Is("Forest"));
          });
    }
  }
}