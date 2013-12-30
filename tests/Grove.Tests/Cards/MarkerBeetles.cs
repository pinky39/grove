namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MarkerBeetles
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void FaeriesGet11()
      {        
        Battlefield(P1, "Cloud of Faeries", "Marker Beetles", "Forest", "Forest");
        Battlefield(P2, "Wall of Blossoms");
        
        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}