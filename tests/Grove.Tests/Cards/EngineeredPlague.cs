namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public partial class EngineeredPlague
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveElvesM1M1()
      {
        Hand(P1, "Engineered Plague");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Unworthy Dead");
        Battlefield(P2, "Llanowar Elves", "Llanowar Elves");

        RunGame(1);

        Equal(1, P1.Battlefield.Creatures.Count());
        Equal(0, P2.Battlefield.Count);
        Equal(2, P2.Graveyard.Count);
      }

      [Fact]
      public void TakeAwayM1M1()
      {
        var plague = C("Engineered Plague");

        Hand(P1, plague);
        Hand(P2, "Naturalize", "Llanowar Elves", "Llanowar Elves");
        
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Unworthy Dead");
        Battlefield(P2, "Llanowar Elves", "Llanowar Elves", "Forest", "Forest", "Forest", "Forest");

        RunGame(2);
        
        Equal(2, P2.Battlefield.Creatures.Count());
        Equal(3, P2.Graveyard.Count);
        Equal(Zone.Graveyard, C(plague).Zone);
      }     
    }
  }
}