namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HighSentinelsOfArashin
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PumpThenAttack()
      {
        Battlefield(P1, "High Sentinels of Arashin", "Plains", "Plains", "Plains", "Plains", "Grizzly Bears");
        P2.Life = 7;

        RunGame(1);

        Assert.Equal(0, P2.Life);
      }
    }
  }
}