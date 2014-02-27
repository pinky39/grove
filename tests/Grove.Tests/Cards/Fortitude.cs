namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Fortitude
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SacForestRegenerateDragon()
      {
        var bolt1 = C("Lightning Bolt");
        var bolt2 = C("Lightning Bolt");
        var dragon = C("Shivan Dragon");

        Battlefield(P2, dragon.IsEnchantedWith("Fortitude"), "Forest");
        Hand(P1, bolt1, bolt2);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt1, target: dragon)
            .Cast(bolt2, target: dragon),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Battlefield, C(dragon).Zone))
          );
      }
    }
  }
}