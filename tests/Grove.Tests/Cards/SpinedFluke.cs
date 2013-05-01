namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class SpinedFluke
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastFluke()
      {
        var fluke = C("Spined Fluke");
        var bears = C("Grizzly Bears");

        Hand(P1, fluke);
        Battlefield(P1, bears, "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Grizzly Bears");

        RunGame(4);

        Equal(Zone.Battlefield, C(fluke).Zone);
        Equal(Zone.Graveyard, C(bears).Zone);
      }
    }
  }
}