namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class IncrementalGrowth
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutThreeCountersOnThreeDifferentTargets()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");
        var bear3 = C("Grizzly Bears");

        Hand(P1, "Incremental Growth");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Forest", "Forest", bear1, bear2, bear3);

        P2.Life = 12;
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
