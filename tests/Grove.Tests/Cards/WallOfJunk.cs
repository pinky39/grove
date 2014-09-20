namespace Grove.Tests.Cards
{
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
        Battlefield(P2, wall, "Swamp", "Swamp");

        P2.Life = 10;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(baloth),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(10, P2.Life);
                Equal(Zone.Hand, C(wall).Zone);
              })
          );
      }
    }
  }
}