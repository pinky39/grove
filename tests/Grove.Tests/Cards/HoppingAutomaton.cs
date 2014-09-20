namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HoppingAutomaton
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackFor1()
      {
        Battlefield(P1, "Hopping Automaton");
        Battlefield(P2, "Verdant Force");
        P2.Life = 1;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}