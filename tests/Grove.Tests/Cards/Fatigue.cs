namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Fatigue
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void OpponentSkipsNextDraw()
      {
        Hand(P1, "Fatigue");
        Battlefield(P1, "Island", "Island");

        RunGame(4);

        Equal(1, P2.Hand.Count);
      }
    }
  }
}