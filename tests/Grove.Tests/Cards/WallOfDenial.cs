namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WallOfDenial
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CannotBeTarget()
      {
        var wall = C("Wall of Denial");

        Hand(P1, "Go for the Throat");
        Battlefield(P2, wall);
        Battlefield(P1, "Swamp", "Swamp");

        RunGame(maxTurnCount: 1);

        Equal(Zone.Battlefield, C(wall).Zone);
      }
    }
  }
}