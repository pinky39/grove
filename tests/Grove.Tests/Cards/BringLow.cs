namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BringLow
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillBear()
      {
        Hand(P1, "Bring Low");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, C("Grizzly Bears").AddCounters(3, CounterType.PowerToughness));

        RunGame(2);

        Assert.Single(P2.Graveyard);
      }
    }
  }
}