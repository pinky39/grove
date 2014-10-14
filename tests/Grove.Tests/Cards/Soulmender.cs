namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Soulmender
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainLife()
      {
        Battlefield(P1, "Soulmender");
        Battlefield(P2, "Wall of Blossoms");
        
        RunGame(2);

        Equal(21, P1.Life);
      }
    }
  }
}