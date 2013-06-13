namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class UnworthyDead
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void BlockAndRegenarate()
      {
        var bears = C("Grizzly Bears");
        var unworthyDead = C("Unworthy Dead");

        Battlefield(P1, bears);
        Battlefield(P2, unworthyDead, "Swamp");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bears),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Battlefield, C(unworthyDead).Zone);
              })
          );
      }
    }
  }
}