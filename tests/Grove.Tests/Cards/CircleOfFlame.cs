namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CircleOfFlame
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DealDamagesToAttackerWithoutFlying()
      {
        Battlefield(P1, "Fugitive Wizard");

        P2.Life = 1;
        Battlefield(P2, "Circle Of Flame");

        RunGame(1);

        Equal(1, P2.Life);
      }

      [Fact]
      public void DoNotDealDamagesToAttackerWithFlying()
      {
        Battlefield(P1, "Angelic Page");

        P2.Life = 1;
        Battlefield(P2, "Circle Of Flame");

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
