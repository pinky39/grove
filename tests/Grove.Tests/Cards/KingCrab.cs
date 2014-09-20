namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class KingCrab
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutForceOnTopOfLibrary()
      {
        var force = C("Verdant Force");
        
        Battlefield(P1, "King Crab", "Island", "Island");        
        Battlefield(P2, force);

        RunGame(1);

        Equal(Zone.Library, C(force).Zone);

      }
    }
  }
}