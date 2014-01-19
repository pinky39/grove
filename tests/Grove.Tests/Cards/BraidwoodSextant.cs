namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class BraidwoodSextant
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SearchForLand()
      {
        Battlefield(P1, "Braidwood Sextant", "Forest", "Forest");
        Library(P1, "Forest");

        RunGame(1);

        Equal(3, P1.Battlefield.Lands.Count());
      }
    }
  }
}