namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Blaze
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal6DamageToPlayer()
      {
        Hand(P1, "Blaze");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 10;

        RunGame(maxTurnCount: 1);

        Equal(4, P2.Life);
      }
    }
  }
}