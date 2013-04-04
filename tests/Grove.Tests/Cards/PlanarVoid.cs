namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class PlanarVoid
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ExileCards()
      {
        Hand(P1, "Stupor");
        Hand(P2, "Island", "Island");
        Battlefield(P1, "Planar Void", "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(2, P2.Exile.Count());        
      }
    }
  }
}