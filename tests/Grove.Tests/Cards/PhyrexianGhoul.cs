namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PhyrexianGhoul
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackToKill()
      {
        Battlefield(P1, "Phyrexian Ghoul", "Llanowar Elves", "Llanowar Elves");
        P2.Life = 6;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}