namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DarkDeal
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EachPlayerDiscardsHandAndDrawsThatManyCardsMinusOne()
      {
        Hand(P1, "Dark Deal");
        Battlefield(P1, "Swamp", "Forest", "Forest");

        Hand(P2, "Swamp", "Forest", "Forest", "Swamp", "Forest", "Forest");

        RunGame(1);

        Equal(6, P2.Graveyard.Count);
        Equal(5, P2.Hand.Count);

        Equal(0, P1.Hand.Count);
      }
    }
  }
}
