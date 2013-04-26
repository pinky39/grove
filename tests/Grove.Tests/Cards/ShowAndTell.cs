namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class ShowAndTell
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutForceOnBattlefield()
      {
        var force = C("Verdant Force");
        var bears = C("Grizzly Bears");

        Hand(P1, force, "Show and Tell");
        Hand(P2, bears);

        Battlefield(P1, "Forest", "Island", "Island");

        RunGame(3);

        Equal(Zone.Battlefield, C(force).Zone);
        Equal(Zone.Graveyard, C(bears).Zone);
      }
    }
  }
}