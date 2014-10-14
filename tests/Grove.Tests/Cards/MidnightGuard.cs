namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MidnightGuard
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackWithGuardThenUntap()
      {
        var guard = C("Midnight Guard");
        
        Hand(P1, "Llanowar Elves");
        Battlefield(P1, guard, "Forest");

        RunGame(1);

        Equal(18, P2.Life);
        False(C(guard).IsTapped);
      }
    }
  }
}