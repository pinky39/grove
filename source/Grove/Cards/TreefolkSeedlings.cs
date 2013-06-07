namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  //public class SerraAvatar : CardsSource
  //{
  //  public override IEnumerable<CardFactory> GetCards()
  //  {
  //    yield return Card
  //      .Named("Serra Avatar")
  //      .ManaCost("{4}{W}{W}{W}")
  //      .Type("Creature Avatar")
  //      .Text("Serra Avatar's power and toughness are each equal to your life total.{EOL}When Serra Avatar is put into a graveyard from anywhere, shuffle it into its owner's library.")
  //      .Power(0)
  //      .Toughness(0)
  //      .Modifier(() => new AddPowerAndToughnessEqualToPlayersLife())

  //  }
  //}
  
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
        .Modifier(() => new ModifyPowerToughnessForEachForest(power: null, toughness: 1, modifier: () => new IntegerSetter()));
    }
  }
}