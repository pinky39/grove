namespace Grove.Tests.Cards
{
  using System.Linq;
  using Grove.Core.Zones;
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
    }
  }
  
  public class TimeWarp
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TakeExtraTurn()
      {
        var warp = C("Time Warp");
        Hand(P1, warp, "Swamp", "Swamp", "Grave Titan");        
        Battlefield(P1, "Island", "Swamp", "Swamp", "Island", "Island", "Rumbling Slum");

        RunGame(2);
        
        Equal(8, P2.Life);
        Equal(Zone.Graveyard, C(warp).Zone);
      }
    }
  }
}