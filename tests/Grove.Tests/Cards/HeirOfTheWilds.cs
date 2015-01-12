namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HeirOfTheWilds
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void HeirGains11OnAttack()
      {
        Battlefield(P1, "Heir of the Wilds", "Juggernaut");

        RunGame(1);

        Equal(12, P2.Life);
      }
    }
  }
}
