namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class TimeSpiral
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastSpiral()
      {
        var spiral = C("Time Spiral");
        Hand(P1, spiral);
        Hand(P2, "Forest", "Forest", "Forest", "Forest");

        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island");
        Graveyard(P2, "Forest");
        Graveyard(P1, "Forest");

        RunGame(1);

        Equal(0, P1.Graveyard.Count);
        Equal(7, P1.Hand.Count);
        Equal(7, P2.Hand.Count);
        Equal(0, P2.Graveyard.Count);
        Equal(6, P1.Battlefield.Lands.Count(x => !x.IsTapped));
        Equal(Zone.Exile, C(spiral).Zone);
      }
    }
  }
}