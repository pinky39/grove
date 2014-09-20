namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RainOfSalt
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Destroy2Lands()
      {
        Hand(P1, "Rain of Salt");
        Battlefield(P1, "Mountain", "Mountain", "Island", "Island", "Island", "Island");
        Battlefield(P2, "Mountain", "Mountain", "Forest");

        RunGame(1);

        Equal(1, P2.Battlefield.Lands.Count());
      }
    }
  }
}