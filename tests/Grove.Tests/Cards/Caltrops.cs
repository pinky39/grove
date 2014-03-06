namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Caltrops
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DoNotAttackWithPegasus()
      {
        Battlefield(P1, "Caltrops", "Pegasus Charger");
        P2.Life = 2;

        RunGame(1);
        Equals(2, P2.Life);
      }
    }
  }
}