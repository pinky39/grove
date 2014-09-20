namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SimianGrunts
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillBears()
      {
        var bears = C("Grizzly Bears");
        Battlefield(P1, bears);
        Battlefield(P2, "Forest", "Forest", "Forest");
        Hand(P2, "Simian Grunts");

        RunGame(1);
        Equal(Zone.Graveyard, C(bears).Zone);
      }
    }
  }
}