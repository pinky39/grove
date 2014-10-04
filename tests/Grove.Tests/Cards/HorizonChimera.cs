namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HorizonChimera
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawCardGetLife()
      {
        Library(P1, "Island");
        Battlefield(P1, "Horizon Chimera");

        Library(P2, "Island");

        // Turn 1: P1 attacks with chimera. P2.Life = 17
        // Turn 2: P2 draws card. P1.Life = 20, P2.Life = 17;
        // Turn 3: P1 draws card. P1.Life = 21. P1 attacks with chimera. P2.Life = 14
        RunGame(3);

        Equal(21, P1.Life);
        Equal(14, P2.Life);
      }
    }
  }
}
