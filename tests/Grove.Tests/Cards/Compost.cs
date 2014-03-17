namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Compost
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawCard()
      {
        Hand(P1, "Ravenous Rats");
        Battlefield(P1, "Compost", "Forest", "Swamp");
        Hand(P2, "Duress", "Duress");

        RunGame(1);

        Equal(1, P2.Hand.Count);
        Equal(1, P1.Hand.Count);
      }
    }
  }
}