namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ParagonOfOpenGraves
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveDebaserDeathtouchToKillDragon()
      {
        var dragon = C("Shivan Dragon");

        Battlefield(P1, "Paragon of Open Graves", "Phyrexian Debaser", "Swamp", "Swamp", "Swamp");        
        Battlefield(P2, dragon);
        
        P2.Life = 3;

        RunGame(3);

        Equal(Zone.Graveyard, C(dragon).Zone);        
      }
    }
  }
}