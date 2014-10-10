namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class LeechingSliver
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackWithSliversOpponentLosesLife()
      {
        Battlefield(P1, "Leeching Sliver", "Leeching Sliver");

        Battlefield(P2, C("Leeching Sliver").IsEnchantedWith("Pacifism"));

        P2.Life = 6;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
