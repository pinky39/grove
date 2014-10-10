namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AggressiveMining
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void YouCannotPlayLands()
      {
        Battlefield(P1, "Aggressive Mining");
        Hand(P1, "Grizzly Bears", "Forest");

        RunGame(1);

        Equal(2, P1.Hand.Count);
      }
    }
  }
}
