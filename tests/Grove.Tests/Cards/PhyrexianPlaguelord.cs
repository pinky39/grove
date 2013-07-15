namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PhyrexianPlaguelord
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyElvesAndAttack()
      {
        Battlefield(P1, "Llanowar Elves", "Phyrexian Plaguelord");
        Battlefield(P2, "Llanowar Elves");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}