namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  
  public class AboutFace
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SwitchToKill()
      {
        Hand(P1, "About Face");
        Battlefield(P1, "Treefolk Seedlings", "Mountain", "Forest", "Forest", "Forest", "Forest", "Forest");

        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void KillWall()
      {
        Hand(P1, "About Face");

        Battlefield(P1, "Mountain", "Grizzly Bears");
        Battlefield(P2, "Wall of Junk");

        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}