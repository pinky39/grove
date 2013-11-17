namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ThievingMagpie
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawCard()
      {
        Battlefield(P1, "Thieving Magpie");

        RunGame(1);
        Equal(1, P1.Hand.Count);
      }
    }
  }
}