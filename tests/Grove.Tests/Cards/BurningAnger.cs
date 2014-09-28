namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BurningAnger
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DealDamageEqualPower()
      {
        Battlefield(P1, "Grizzly Bears", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Hand(P1, "Burning Anger");

        Battlefield(P2, "Wall of Fire");
        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}