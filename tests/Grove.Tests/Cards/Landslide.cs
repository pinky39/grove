namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Landslide
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal5DamageToOpponent()
      {
        Hand(P1, "Landslide");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        P2.Life = 0;
        
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}