namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GreaterGood
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SacForce()
      {
        var force = C("Verdant Force");
        var blade = C("Doom blade");

        Battlefield(P2, force, "Greater Good");
        Hand(P1, blade);

        Exec(
          At(Step.FirstMain)
            .Cast(blade, target: force),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(force).Zone);
                Equal(4, P2.Hand.Count);
              })
          );
      }
    }
  }
}