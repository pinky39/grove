namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class YawgmothsBargain
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Draw7Cards()
      {
        Battlefield(P1, "Yawgmoth's Bargain");
        P1.Life = 21;

        RunGame(1);
        
        Equal(14, P1.Life);
        Equal(7, P1.Hand.Count);
      }
    }
  }
}