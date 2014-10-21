namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WallOfMulch
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacWallToDraw()
      {
        Battlefield(P1, "Ravenous Baloth", "Ravenous Baloth");
        Battlefield(P2, "Wall of Mulch", "Wall of Blossoms", "Forest", "Forest");
        P2.Life = 8;
        
        RunGame(3);

        Equal(0, P2.Life);
        Equal(2, P2.Graveyard.Count);
        Equal(3, P2.Hand.Count);        
      }
    }
  }
}