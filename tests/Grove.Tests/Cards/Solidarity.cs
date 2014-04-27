namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Solidarity
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Block()
      {
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
        Battlefield(P2, "Giant Cockroach", "Giant Cockroach", "Giant Cockroach", "Plains", "Swamp", "Swamp" ,"Swamp");

        Hand(P2, "Solidarity");

        RunGame(1);

        Equal(3, P1.Graveyard.Creatures.Count());
        Equal(0, P2.Graveyard.Creatures.Count());
      }
    }
  }
}