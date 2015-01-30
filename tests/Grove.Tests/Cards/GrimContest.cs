namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class GrimContest
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void FightWallAndBear()
      {
        Hand(P1, "Grim Contest");
        Battlefield(P1, "Grizzly Bears", "Wall of Frost", "Forest", "Forest", "Swamp");

        Battlefield(P2, "Grizzly Bears", "Forest", "Forest", "Swamp");

        RunGame(1);

        Equal(1, P1.Graveyard.Count);
        Equal(1, P2.Graveyard.Count);
      }
    }
  }
}
