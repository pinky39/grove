namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class PowerSink
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CounterForce()
      {
        var force = C("Verdant Force");

        Hand(P1, force);
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        Hand(P2, "Power Sink");
        Battlefield(P2, "Island", "Island", "Island");

        RunGame(1);

        Equal(Zone.Graveyard, C(force).Zone);
        Equal(8, P1.Battlefield.Lands.Count(x => x.IsTapped));        
      }
    }
  }
}