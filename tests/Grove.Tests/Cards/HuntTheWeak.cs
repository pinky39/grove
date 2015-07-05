namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HuntTheWeek
  {
    public  class Ai : AiScenario
    {
      [Fact]
      public void KillDragonWithDragon()
      {
        var dragon1 = C("Shivan Dragon");
        var dragon2 = C("Shivan Dragon");
        
        Hand(P1, "Hunt the Weak");
        
        Battlefield(P1, dragon1, "Mountain", "Mountain", "Mountain", "Forest");
        Battlefield(P2, "Grizzly Bears", dragon2);

        RunGame(1);

        Equal(Zone.Battlefield, C(dragon1).Zone);
        Equal(Zone.Graveyard, C(dragon2).Zone);
        Equal(14, P2.Life);
      }

      [Fact]
      public void RescueDragonFromFightItShouldNotGetInvalidTargetBug()
      {
        var dragon1 = C("Shivan Dragon");
        var dragon2 = C("Shivan Dragon");

        Hand(P1, "Hunt the Weak");
        Battlefield(P1, dragon1, "Forest", "Forest", "Forest", "Forest");

        Hand(P2, "Rescue");
        Battlefield(P2, dragon2, "Island");

        RunGame(2);

        Equal(Zone.Battlefield, C(dragon1).Zone);
        Equal(Zone.Hand, C(dragon2).Zone);
        Equal(14, P2.Life);
      }
    }
  }
}