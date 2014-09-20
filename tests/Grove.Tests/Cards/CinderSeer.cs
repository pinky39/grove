namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CinderSeer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillPlayer()
      {

        Hand(P1, "Shivan Dragon", "Shivan Dragon", "Shivan Dragon", "Nantuko Shade");
        Battlefield(P1, "Cinder Seer", "Mountain", "Mountain", "Mountain");

        P2.Life = 3;

        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}