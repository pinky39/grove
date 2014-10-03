namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class LightningStrike
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillOpponent()
      {
        Hand(P1, "Lightning Strike");
        Battlefield(P1, "Grizzly Bears", "Mountain", "Mountain");

        P2.Life = 3;
        Battlefield(P2, "Wall of Fire");

        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}