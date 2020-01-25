namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CracklingDoom
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void OpponentSacsDragon()
      {
        Hand(P1, "Crackling Doom");
        Battlefield(P1, "Shivan Dragon", "Mountain", "Plains", "Swamp");
        Battlefield(P2, "Shivan Dragon", "Grizzly Bears");

        P2.Life = 7;

        RunGame(1);

        Assert.Equal(0, P2.Life);
      }
    }
  }
}