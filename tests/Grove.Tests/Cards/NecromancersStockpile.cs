namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class NecromancersStockpile
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawCardGetToken()
      {        
        Hand(P1, "Gravedigger", "Llanowar Elves");
        Battlefield(P1, "Necromancer's Stockpile", "Swamp", "Plains");
        
        RunGame(2);

        Equal(2, P1.Hand.Count);
        Equal(1, P1.Battlefield.Creatures.Count(c => c.Is().Token));
      }

      [Fact]
      public void DrawCardDontGetToken()
      {        
        Hand(P1, "Juggernaut", "Juggernaut");
        Battlefield(P1, "Necromancer's Stockpile", "Swamp", "Plains");

        RunGame(1);

        Equal(2, P1.Hand.Count);
        Equal(0, P1.Battlefield.Creatures.Count(c => c.Is().Token));        
      }
    }
  }
}
