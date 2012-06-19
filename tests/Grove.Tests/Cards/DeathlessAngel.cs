namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class DeathlessAngel
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void IndestructibleShield()
      {
        var angel = C("Deathless Angel");
        var blade = C("Doom blade");

        Battlefield(P2, angel, "Plains", "Plains");
        Hand(P1, blade);

        Exec(
          At(Step.FirstMain)
            .Cast(blade, target: angel)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(angel).Zone);
                True(C(angel).Has().Indestructible);
              }
            ));
      }
    }
  }
}