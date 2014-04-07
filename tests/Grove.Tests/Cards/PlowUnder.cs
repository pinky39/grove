namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class PlowUnder
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutOnTop()
      {
        Hand(P1, "Plow Under");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Mountain", "Mountain");

        RunGame(1);

        Equal(0, P2.Battlefield.Count(c => c.Is().Land));
      }
    }
  }
}