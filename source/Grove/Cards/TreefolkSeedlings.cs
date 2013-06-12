namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class TreefolkSeedlings : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.Modifier(() => new ModifyPowerToughnessForEachForest(
              power: null,
              toughness: 1,
              modifier: () => new IntegerSetter()));

            p.EnabledInAllZones = true;
          });
    }
  }
}