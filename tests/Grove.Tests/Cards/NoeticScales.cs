namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class NoeticScales
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnCreatures()
      {
        var force = C("Verdant Force");
        var dragon = C("Shivan Dragon");
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");

        Battlefield(P1, force, "Noetic Scales", bear1);
        Battlefield(P2, dragon, bear2);

        Hand(P1, "Swamp", "Swamp", "Swamp");
        Hand(P2, "Swamp", "Swamp", "Swamp");

        RunGame(2);

        Equal(Zone.Hand, C(force).Zone);
        Equal(Zone.Hand, C(dragon).Zone);
        Equal(Zone.Battlefield, C(bear1).Zone);
        Equal(Zone.Battlefield, C(bear2).Zone);
      }
    }
  }
}