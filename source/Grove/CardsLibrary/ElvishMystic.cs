namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class ElvishMystic : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Elvish Mystic")
        .ManaCost("{G}")
        .Type("Creature - Elf Druid")
        .Text("{T}: Add {G} to your mana pool. ")
        .FlavorText("\"Life grows everywhere. My kin merely find those places where it grows strongest.\"{EOL}—Nissa Revane")
        .Power(1)
        .Toughness(1)
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {G} to your mana pool.";
          p.ManaAmount(Mana.Green);
        });
    }
  }
}
