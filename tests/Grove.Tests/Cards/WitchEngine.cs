namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WitchEngine
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ActivateEngine()
      {
        Hand(P1, "Beacon of Destruction");
        Battlefield(P1, "Witch Engine", "Mountain", "Mountain");
        Battlefield(P2, "Wall of Denial");
        P2.Life = 5;

        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}