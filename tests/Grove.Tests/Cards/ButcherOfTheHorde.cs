namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ButcherOfTheHorde
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainHaste()
      {
        Hand(P1, "Butcher of the Horde");
        Battlefield(P1, "Grizzly Bears", "Mountain", "Mountain", "Plains", "Swamp");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}