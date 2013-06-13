namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TreetopRangers
  {
    public class Ai :AiScenario
    {
      [Fact]
      public void CanBeBlockedOnlyByFlyingCreatures()
      {
        Battlefield(P1, "Treetop Rangers");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}