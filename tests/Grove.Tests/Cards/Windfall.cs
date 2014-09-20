namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Windfall
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Draw7()
      {
        Hand(P1, "Windfall", "Island");
        Hand(P2, "Island", "Island", "Island", "Island", "Island", "Island", "Island");

        Battlefield(P1, "Island", "Island");

        RunGame(1);

        Equal(7, P1.Hand.Count);
        Equal(7, P2.Hand.Count);
      }
    }
  }
}