namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class TripNoose
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TapBlocker()
      {
        Battlefield(P1, "Forest", "Forest", "Trip Noose", "Rumbling Slum");
        Battlefield(P2, "Grizzly Bears");
        P2.Life = 6;
        
        RunGame(1);

        Equal(1, P2.Battlefield.Creatures.Count());
        Equal(0, P2.Life);        
      }

      [Fact]
      public void TripNoosePickToughestTarget()
      {                
        Battlefield(P1, "Student of Warfare", "Birds of Paradise", "Birds of Paradise");
        Battlefield(P2, "Island", "Island", "Trip Noose");

        RunGame(1);
        
        Equal(20, P2.Life);        
      }
    }
  }
}