namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class NightshadeSeer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BearGetsM2M2()
      {
        Hand(P1, "Giant Cockroach", "Giant Cockroach");
        Battlefield(P1, "Nightshade Seer", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(1, P2.Graveyard.Count);
      }
    }
  }
}