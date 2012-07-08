namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class WallOfJunk
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void ReturnToOwnersHand()
      {
        var wall = C("Wall of Junk");
        var baloth = C("Leatherback Baloth");

        Battlefield(P1, baloth);
        Battlefield(P2, wall);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(baloth),
          At(Step.SecondMain)
            .Verify(()=> Equal(Zone.Hand, C(wall).Zone))
          );
      }
    }
  }
}