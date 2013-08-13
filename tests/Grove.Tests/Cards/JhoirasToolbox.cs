namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class JhoirasToolbox
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BalothCantDealDamage()
      {
        Battlefield(P1, "Leatherback Baloth");
        Battlefield(P2, "Jhoira's Toolbox", "Forest", "Forest");

        P2.Life = 4;
        RunGame(1);

        Equal(4, P2.Life);
      }
    }
  }
}