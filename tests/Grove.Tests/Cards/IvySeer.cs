namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class IvySeer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PumpBear()
      {
        Battlefield(P1, "Grizzly Bears", "Ivy Seer", "Forest", "Forest", "Forest");
        Hand(P1, "Verdant Force", "Verdant Force", "Verdant Force");

        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}