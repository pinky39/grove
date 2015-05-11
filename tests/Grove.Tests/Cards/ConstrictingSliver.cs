namespace Grove.Tests.Cards
{
  using Xunit;
   using Infrastructure;

  public class ConstrictingSliver
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlaySliverExileDragon()
      {
        var dragon = C("Shivan Dragon");

        Hand(P1, "Venom Sliver");        
        Battlefield(P1, "Constricting Sliver", "Forest", "Forest");                
        Battlefield(P2, dragon);

        RunGame(1);
        
        Equal(Zone.Exile, C(dragon).Zone);
      }

      [Fact]
      public void PlaySelfExileDragon()
      {
        var dragon = C("Shivan Dragon");
        var sliver = C("Constricting Sliver");

        Hand(P1, sliver);
        Battlefield(P1, "Plains", "Forest", "Forest", "Plains", "Forest", "Forest");
        Battlefield(P2, dragon);

        RunGame(1);

        Equal(Zone.Battlefield, C(sliver).Zone);
        Equal(Zone.Exile, C(dragon).Zone);
      }

      [Fact]
      public void ReturnDragonToPlay()
      {
        var dragon = C("Shivan Dragon");
        var sliver = C("Venom Sliver");

        Hand(P1, sliver);
        Battlefield(P1, "Constricting Sliver", "Forest", "Forest");
        
        Hand(P2, "Shock");
        Battlefield(P2, dragon, "Mountain");

        RunGame(2);

        Equal(Zone.Graveyard, C(sliver).Zone);
        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }

  }
}