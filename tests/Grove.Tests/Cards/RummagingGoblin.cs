namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RummagingGoblin
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DiscardIsland()
      {
        Hand(P1, "Island", "Island");
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island",
          "Island", "Rummaging Goblin");

        Battlefield(P2, "Wall of Blossoms");

        RunGame(2);

        Equal(1, P1.Graveyard.Count(x => x.Is("island")));
      }
    }
  }
}