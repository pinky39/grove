namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Scrapheap
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Gain1LifeForRing()
      {
        Battlefield(P1, "Scrapheap", "Ring of Gix");

        RunGame(1);
        Equal(21, P1.Life);
      }
    }
  }
}