namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Flicker
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KeldonAndFlicker()
      {
        Hand(P1, "Keldon Champion", "Flicker");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Plains", "Plains", "Plains");
        P2.Life = 9;

        RunGame(1);
        
        Equal(0, P2.Life);
      }
    }
  }
}