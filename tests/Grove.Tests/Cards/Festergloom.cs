namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Festergloom
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReduceToughnessAndAttack()
      {
        Hand(P1, "Festergloom");
        Battlefield(P1, "Trained Armodon", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Llanowar Elves");
        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}