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
        Battlefield(P1, "Witch Engine", "Pestilence");
        Battlefield(P2, "Wall of Denial");
        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}