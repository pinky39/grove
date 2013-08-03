namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Archivist
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawACard()
      {
        Battlefield(P1, "Archivist");
        RunGame(1);

        Equal(1, P1.Hand.Count);
      }
    }
  }
}