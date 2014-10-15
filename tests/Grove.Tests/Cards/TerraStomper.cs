namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class TerraStomper
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CannotBeCountered()
      {
        Hand(P1, "Terra Stomper");

        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");

        Hand(P2, "Counterspell");
        Battlefield(P2, "Island", "Island");

        RunGame(1);

        Equal(1, P1.Battlefield.Creatures.Count());
      }
    }
  }
}
