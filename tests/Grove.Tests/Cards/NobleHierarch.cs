namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class NobleHierarch
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void NobleGains11WhenAttacks()
      {
        Battlefield(P1, "Noble Hierarch");

        P2.Life = 1;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void BearsAttackWithoutEnchancement()
      {
        Battlefield(P1, "Noble Hierarch", "Grizzly Bears", "Grizzly Bears");

        P2.Life = 5;

        RunGame(1);

        Equal(1, P2.Life);
      }
    }
  }
}
