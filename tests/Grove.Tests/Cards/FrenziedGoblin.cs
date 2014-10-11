namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class FrenziedGoblin
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TapBearOnAttack()
      {
        Battlefield(P1, "Frenzied Goblin", "Mountain");

        P2.Life = 1;
        Battlefield(P2, "Grizzly Bears", "Swamp");

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
