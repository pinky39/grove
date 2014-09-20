namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class GreenerPastures
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ControllerGetsToken()
      {
        Battlefield(P1, "Greener Pastures", "Forest");

        RunGame(2);

        Equal(1, P1.Battlefield.Creatures.Count());
        Equal(0, P2.Battlefield.Creatures.Count());
      }

      [Fact]
      public void OpponentGetsToken()
      {
        Battlefield(P1, "Greener Pastures");
        Battlefield(P2, "Forest");

        RunGame(2);

        Equal(0, P1.Battlefield.Creatures.Count());
        Equal(1, P2.Battlefield.Creatures.Count());
      }
    }
  }
}