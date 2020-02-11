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
        Hand(P1, "Dark Deal", "Island", "Island", "Island", "Island", "Island", "Island");
        Battlefield(P1, "Swamp", "Forest", "Forest");

        Hand(P2, "Swamp");

        RunGame(1);
        
        Equal(0, P2.Hand.Count);
        Equal(4, P1.Hand.Count);
      }
    }
  }
}
