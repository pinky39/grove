namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class WallOfLimbs
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AddCountersToWallSacToKillPlayer()
      {
        var wallOfLimbs = C("Wall of Limbs");

        Hand(P1, "Radiant's Dragoons");        
        Battlefield(P1, wallOfLimbs, "Plains", "Plains", "Plains", "Swamp", 
          "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Plains", "Plains");
        
        P2.Life = 1;
        
        RunGame(2);
                        
        Equal(0, P2.Life);        
        Equal(Zone.Graveyard, C(wallOfLimbs).Zone);
      }
    }
  }
}