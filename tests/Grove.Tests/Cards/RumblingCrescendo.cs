namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RumblingCrescendo
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Destroy3Lands()
      {
        Battlefield(P1, "Rumbling Crescendo", "Mountain");
        Battlefield(P2, "Island", "Island", "Mountain", "Mountain", "Grizzly Bears");

        RunGame(5);

        Equal(1, P2.Battlefield.Lands.Count());
      }
    }
  }
}