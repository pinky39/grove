namespace Grove.Tests.Cards
{
  using Core;
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class ThrunTheLastTroll
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CannotBeTarget()
      {
        var blade = C("Doom Blade");
        var bear = C("Grizzly Bears");
        var thrun = C("Thrun, the Last Troll");

        Battlefield(P1, thrun);
        Battlefield(P2, bear, "Swamp", "Swamp");

        Hand(P2, blade);

        RunGame(maxTurnCount: 2);

        Equal(Zone.Battlefield, C(thrun).Zone);
        Equal(Zone.Hand, C(blade).Zone);
        Equal(Zone.Battlefield, C(bear).Zone);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Regenerate()
      {
        var thrun = C("Thrun, the Last Troll");
        var shock1 = C("Shock");
        var shock2 = C("Shock");

        Battlefield(P1, thrun);
        Hand(P1, shock1, shock2);

        Exec(
          At(Step.FirstMain)
            .Cast(shock1, target: thrun)
            .Cast(shock2, target: thrun)
            .Activate(thrun)
            .Verify(() =>
              {
                Equals(Zone.Battlefield, C(thrun).Zone);
                True(C(thrun).IsTapped);
                Equals(0, C(thrun).Damage);
              })
          );
      }
    }
  }
}