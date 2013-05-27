namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class SpreadingAlgae
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantSwamp()
      {
        Hand(P1, "Spreading Algae");
        Hand(P2, "Nantuko Shade");
        
        Battlefield(P1, "Forest");
        Battlefield(P2, "Swamp", "Swamp");

        P1.Life = 3;
        
        RunGame(4);
        
        Equal(2, P2.Graveyard.Count(x => x.Is("swamp")));
      }
    }
  }
}