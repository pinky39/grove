namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Gamekeeper
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutForceIntoPlay()
      {
        var force = C("Verdant Force");
        var forest = C("Forest");
        var gamekeeper = C("Gamekeeper");

        Library(P1, forest, force, "Swamp");        
        Battlefield(P1, gamekeeper);
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 2;

        RunGame(1);

        Equal(Zone.Battlefield, C(force).Zone);
        Equal(Zone.Graveyard, C(forest).Zone);
        Equal(Zone.Exile, C(gamekeeper).Zone);
      }
    }
  }
}