namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class PlanarBirth
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnLands()
      {
        Hand(P1, "Planar Birth");

        Battlefield(P1, "Plains", "Forest");
        Battlefield(P2, "Swamp");
        Graveyard(P1, "Forest", "Forest", "Forest");
        Graveyard(P2, "Swamp");

        RunGame(1);

        Equal(5, P1.Battlefield.Lands.Count());
        Equal(2, P2.Battlefield.Lands.Count());
      }
    }
  }
}