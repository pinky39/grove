namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Overwhelm
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackFor10()
      {
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Wall of Frost", "Forest", "Plains", "Plains", "Plains",
          "Forest", "Plains");
        Hand(P1, "Overwhelm");

        P2.Life = 10;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}