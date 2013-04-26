namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class BlanchwoodArmor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BugElvesWithArmorGetTapedForManaInsteadOfAttacking()
      {
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Llanowar elves");
        Hand(P1, "Blanchwood armor");

        RunGame(maxTurnCount: 2);

        Assert.Equal(15, P2.Life);
      }

      [Fact]
      public void EnchantCreature()
      {
        var armor = C("Blanchwood armor");

        Battlefield(P1, "Forest", "Forest", "Forest", "Grizzly Bears");
        Hand(P1, armor);
        Battlefield(P2, "Forest");

        RunGame(maxTurnCount: 2);

        Assert.True(P1.Battlefield.Contains(C(armor)));
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void CastArmorOnOpponentCreature()
      {
        var forest = C("Forest");
        var armor = C("Blanchwood Armor");
        var bear = C("Grizzly Bears");

        Battlefield(P2, bear);
        Hand(P1, forest, armor);

        Exec(
          At(Step.FirstMain)
            .Cast(armor, target: bear)
            .Cast(forest)
            .Verify(() =>
              {
                Equal(3, C(bear).Toughness);
                Equal(3, C(bear).Power);
                True(P2.Battlefield.Contains(armor));
              }));
      }

      [Fact]
      public void EnchantedCreatureGets11ForEachForestPlayerControls()
      {
        var forest = C("Forest");
        var armor = C("Blanchwood Armor");
        var bear = C("Grizzly Bears");

        Battlefield(P1, C("Forest"), bear);
        Hand(P1, forest, armor);

        Exec(
          At(Step.FirstMain)
            .Cast(armor, target: bear)
            .Cast(forest)
            .Verify(() =>
              {
                Equal(4, C(bear).Power);
                Equal(4, C(bear).Toughness);
              }));
      }

      [Fact]
      public void WhenCreatureGoesToGraveyardEnchantmentGoesToGraveyardToo()
      {
        var forest = C("Forest");
        var hammer = C("Volcanic Hammer");
        var armor = C("Blanchwood Armor");
        var bear = C("Grizzly Bears");

        Battlefield(P2, forest, bear.IsEnchantedWith(armor));
        Hand(P1, hammer);

        Exec(
          At(Step.FirstMain)
            .Cast(hammer, target: bear)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(armor).Zone);
                Equal(Zone.Graveyard, C(bear).Zone);
                Equal(2, C(bear).Toughness);
                Equal(2, C(bear).Power);
              })
          );
      }
    }
  }
}