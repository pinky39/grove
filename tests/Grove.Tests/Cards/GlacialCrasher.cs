namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GlacialCrasher
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CantAttack()
      {
        Battlefield(P1, "Glacial Crasher");
        P2.Life = 5;

        RunGame(1);
        Equal(5, P2.Life);
      }

      [Fact]
      public void CanAttack()
      {
        Hand(P1, "Mountain");
        Battlefield(P1, "Glacial Crasher");
        P2.Life = 5;

        RunGame(1);
        Equal(0, P2.Life);
      }
    }
  }
}