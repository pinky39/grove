namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class StrokeOfGenius
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Draw3Cards()
      {
        Hand(P1, "Stroke of Genius");
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island");

        RunGame(2);

        Equal(3, P1.Hand.Count);
      }
    }
  }
}