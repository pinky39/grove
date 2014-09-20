namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Modifiers;

  public class TreefolkSeedlings : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treefolk Seedlings")
        .ManaCost("{2}{G}")
        .Type("Creature Treefolk")
        .Text("Treefolk Seedlings's toughness is equal to the number of Forests you control.")
        .FlavorText(
          "The year that the brothers landed on Argoth, the trees produced five times as many seeds as normal.")
        .Power(2)
        .Toughness(0)
        .StaticAbility(p =>
          {
            p.Modifier(() => new ModifyPowerToughnessForEachPermanent(
              power: null,
              toughness: 1,
              filter: (c, _) => c.Is("forest"),
              modifier: () => new IntegerSetter()));

            p.EnabledInAllZones = true;
          });
    }
  }
}