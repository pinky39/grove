namespace Grove.Tests.Cards
{
  using System.Linq;
  using Grove.Core;
  using Infrastructure;
  using Xunit;

  public class Shock
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BugCreatureWithTougnessGreaterThan2GetsShocked()
      {
        var shock = C("Shock");
        var elves = C("Elvish Warrior");

        Battlefield(P1, elves);
        Battlefield(P2, "Mountain", "Mountain");
        Hand(P2, shock);

        RunGame(maxTurnCount: 2);

        True(P1.Battlefield.Contains(elves));
      }

      [Fact]
      public void BugDoNotShockPlayerWhenLifeIsHigh()
      {
        var shock = C("Shock");
        Battlefield(P1, "Mountain");
        Hand(P1, shock);

        RunGame(maxTurnCount: 2);

        Assert.True(P1.Hand.Contains(shock));
      }

      [Fact]
      public void DoNotShockCreaturesWithToughness3OrMore()
      {
        var shock = C("Shock");

        Battlefield(P1, "Elvish Warrior");
        Battlefield(P2, "Mountain");
        Hand(P2, shock);

        RunGame(maxTurnCount: 2);

        True(P2.Hand.Contains(shock));
      }

      [Fact]
      public void ShockCreaturesWithTougness2OrLess()
      {
        var shock = C("Shock");
        var bear = C("Grizzly Bears");

        Battlefield(P1, bear);
        Battlefield(P2, "Mountain");
        Hand(P2, shock);

        RunGame(maxTurnCount: 2);

        False(P2.Hand.Contains(shock));
        False(P1.Battlefield.Contains(bear));
      }

      [Fact]
      public void ShockPlayerWhenLifeIsLow()
      {
        var shock = C("Shock");

        Battlefield(P1, "Mountain");
        Hand(P1, shock);

        P2.Life = 5;

        RunGame(maxTurnCount: 2);

        False(P1.Hand.Contains(shock));
        Equal(3, P2.Life);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DealDamageToCreatureInResponseToEnchantment()
      {
        var forest = C("Forest");
        var shock = C("Shock");
        var bear = C("Grizzly Bears");
        var armor = C("Blanchwood Armor");

        Battlefield(P1, bear, forest);
        Hand(P1, armor);
        Hand(P2, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(armor, target: bear)
            .Cast(shock, target: bear),
          At(Step.SecondMain)
            .Verify(() => {
              Equal(2, P1.Graveyard.Count());
              Equal(1, P2.Graveyard.Count());
              Equal(2, C(bear).Power);
            }));
      }

      [Fact]
      public void DealDamageToPlayer()
      {
        var shock = C("Shock");

        Hand(P1, shock);

        Exec(
          At(Step.Upkeep)
            .Cast(shock, target: P2)
            .Verify(() => Assert.Equal(18, P2.Life)));
      }

      [Fact]
      public void KillCreatureInResponse()
      {
        var shock1 = C("Shock");
        var shock2 = C("Shock");
        var behemoth = C("Llanowar Behemoth");

        Hand(P1, shock1, shock2);
        Battlefield(P2, behemoth);

        Exec(
          At(Step.FirstMain)
            .Cast(shock1, target: behemoth)
            .Cast(shock2, target: behemoth)
            .Activate(behemoth, costTarget: behemoth)
            .Verify(() =>
              Equal(1, P2.Battlefield.Count()))
          );
      }
    }
  }
}