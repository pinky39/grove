namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BorderlandMarauder
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void IncreasePowerBy2OnAttack()
      {
        Battlefield(P1, "Borderland Marauder");

        RunGame(1);

        Equal(17, P2.Life);
      }
    }
  }
}
