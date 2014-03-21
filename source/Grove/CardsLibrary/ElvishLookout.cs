namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class ElvishLookout : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Elvish Lookout")
        .ManaCost("{G}")
        .Type("Creature Elf")
        .Text("{Shroud} (This permanent can't be the target of spells or abilities.)")
        .FlavorText(
          "Like a masterful quilter, Yavimaya stitches together patches of color to warn its denizens of threats.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Shroud);
    }
  }
}