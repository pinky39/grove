namespace Grove.Tests.Cards
{
  using Gameplay;
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

      [Fact]
      public void PutAuraOnBattlefield()
      {
        var force = C("Verdant Force");
        var bears = C("Grizzly Bears");
        var rancor = C("Rancor");

        Hand(P1, force, "Show and Tell");
        Hand(P2, rancor);

        Battlefield(P1, "Forest", "Island", "Island");
        Battlefield(P2, bears);

        RunGame(1);

        Equal(Zone.Battlefield, C(force).Zone);
        Equal(Zone.Battlefield, C(rancor).Zone);
        Equal(C(bears), C(rancor).AttachedTo);
      }
      
    }
  }
}