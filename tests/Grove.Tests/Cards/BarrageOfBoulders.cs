namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BarrageOfBoulders
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Ferocious()
      {
        var elves = C("Llanowar Elves");

        Hand(P1, "Barrage of Boulders");
        Battlefield(P1, "Leatherback Baloth", "Mountain", "Forest", "Forest");
        Battlefield(P2, elves, "Grizzly Bears");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
        Equal(Zone.Graveyard, C(elves).Zone);
      }

      [Fact]
      public void NotFerocious()
      {
        var elves1 = C("Llanowar Elves");
        var elves2 = C("Llanowar Elves");

        Hand(P1, "Barrage of Boulders");
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Mountain", "Forest", "Forest");
        Battlefield(P2, elves1, elves2, "Wall of Blossoms");

        P2.Life = 4;

        RunGame(1);

        Equal(2, P2.Life);
        Equal(Zone.Graveyard, C(elves1).Zone);
        Equal(Zone.Graveyard, C(elves2).Zone);
      }
    }
  }
}