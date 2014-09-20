namespace Grove.Tests.Cards
{
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

      [Fact]
      public void AttackWithFluke()
      {
        var fluke = C("Spined Fluke");
        Battlefield(P1, fluke, "Swamp", "Llanowar Elves", "Llanowar Elves", "Llanowar Elves");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        True(C(fluke).IsTapped);
      }
    }
  }
}