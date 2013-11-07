namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ScentOfNightshade
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BearGetsM2M2()
      {
        Hand(P1, "Giant Cockroach", "Giant Cockroach", "Scent of Nightshade");
        Battlefield(P1, "Swamp", "Swamp",  "Giant Cockroach");
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(16, P2.Life);
        Equal(1, P2.Graveyard.Count);
      }
    }
  }
}