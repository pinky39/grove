namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class FodderCannon
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillWallAndWin()
      {
        Battlefield(P1, "Trained Armodon", "Llanowar Elves", "Fodder Cannon", "Island", "Forest", "Island", "Forest");
        Battlefield(P2, "Wall of Blossoms");
        P2.Life = 3;
        
        RunGame(1);

        Equal(0, P2.Life);
        Equal(1, P1.Graveyard.Count);
        Equal(1, P2.Graveyard.Count);
      }
    }
  }
}