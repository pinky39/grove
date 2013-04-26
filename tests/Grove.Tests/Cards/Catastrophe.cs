namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Catastrophe
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyCreatures()
      {
        Hand(P1, "Catastrophe");

        Battlefield(P1, "Grizzly Bears", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Grizzly Bears", "Shivan Dragon");

        P1.Life = 5;

        RunGame(2);

        Equal(0, P1.Battlefield.Creatures.Count());
        Equal(0, P2.Battlefield.Creatures.Count());
      }

      [Fact]
      public void DestroyLands()
      {
        // todo make lands score dependable on other permanents count
        // so cpu will destroy all lands when he has better board position

        Hand(P1, "Catastrophe");

        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains",
          "Plains");

        RunGame(1);

        Equal(0, P1.Battlefield.Lands.Count());
        Equal(0, P2.Battlefield.Lands.Count());
      }
    }
  }
}