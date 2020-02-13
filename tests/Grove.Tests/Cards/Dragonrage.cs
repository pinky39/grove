namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Dragonrage
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PumpBears()
      {
        Hand(P1, "Dragonrage");
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Mountain", "Mountain", "Mountain");

        P2.Life = 6;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}