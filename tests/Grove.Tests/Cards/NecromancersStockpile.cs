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
      public void DiscardZombieDrawCardAndPutToken()
      {
        Library(P1, "Gravedigger", "Gravedigger");
        Hand(P1, "Gravedigger", "Gravedigger");
        Battlefield(P1, "Necromancer's Stockpile", "Swamp", "Plains");
        
        RunGame(1);

        Equal(1, P1.Battlefield.Creatures.Count(c => c.Is().Token));
      }

      [Fact]
      public void DiscardZombieDrawCard()
      {
        Library(P1, "Juggernaut", "Juggernaut");
        Hand(P1, "Juggernaut", "Juggernaut");
        Battlefield(P1, "Necromancer's Stockpile", "Swamp", "Plains");

        RunGame(1);

        Equal(0, P1.Battlefield.Creatures.Count(c => c.Is().Token));
        Equal(1, P1.Graveyard.Count);
      }
    }
  }
}
