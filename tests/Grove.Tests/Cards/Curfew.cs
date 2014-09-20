namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Curfew
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BounceDragon()
      {
        Hand(P2, "Curfew");
        Battlefield(P2, "Island");
        Battlefield(P1, "Shivan Dragon");

        RunGame(1);

        Equal(0, P1.Battlefield.Count());
      }
    }
  }
}