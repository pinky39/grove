namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class LurkingEvil
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ChangeAndAttack()
      {
        Battlefield(P1, "Lurking Evil");
        Battlefield(P2, "Verdant Force");
        P1.Life = 19;
        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
        Equal(9, P1.Life);
      }
    }
  }
}