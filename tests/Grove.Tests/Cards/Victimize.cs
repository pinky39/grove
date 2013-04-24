namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Victimize
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DoNotCastWithOnlyOneCreatureInGraveyard()
      {
        var force = C("Verdant Force");        
        var bear = C("Grizzly bears");
        
        Hand(P1, "Victimize");        
        Graveyard(P1, force);        
        Battlefield(P1, bear, "Swamp", "Forest", "Forest");

        RunGame(1);
        
        Equal(Zone.Graveyard, C(force).Zone);        
      }
      
      [Fact]
      public void BringBack2Creatures()
      {
        var force = C("Verdant Force");
        var dragon = C("Shivan Dragon");
        var bear = C("Grizzly bears");
        
        Hand(P1, "Victimize");        
        Graveyard(P1, force, dragon);        
        Battlefield(P1, bear, "Swamp", "Forest", "Forest");

        RunGame(1);

        Equal(Zone.Battlefield, C(dragon).Zone);
        Equal(Zone.Battlefield, C(force).Zone);
        Equal(Zone.Graveyard, C(bear).Zone);
      }
    }    
  }
}