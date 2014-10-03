namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Gravedigger
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutBearToHand()
      {
        Hand(P1, "Gravedigger");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Grizzly Bears");
        Graveyard(P1, "Grizzly Bears");

        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count());
        Equal(1, P1.Hand.Count());
        Equal(0, P1.Graveyard.Count());
      }
    }
  }
}