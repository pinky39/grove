namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ReturnToTheRanks
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnBearsFromGraveyard()
      {
        Hand(P1, "Return To The Ranks");
        Graveyard(P1, "Grizzly Bears", "Grizzly Bears");
        Battlefield(P1, "Plains", "Plains", "Plains", "Wall of Frost");

        RunGame(1);

        Equal(3, P1.Battlefield.Creatures.Count());
      }
    }
  }
}
