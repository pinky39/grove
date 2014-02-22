namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class ManOWar
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BounceForce()
      {
        var force = C("Verdant Force");
        var manOWar = C("Man-o'-War");

        Hand(P1, manOWar);
        Battlefield(P1, "Island", "Island", "Island");
        Battlefield(P2, force);

        RunGame(1);

        Equal(Zone.Battlefield, C(manOWar).Zone);
        Equal(Zone.Hand, C(force).Zone);
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {     
      [Fact]
      public void BounceNoOne()
      {
        var force = C("Verdant Force");
        var manOWar = C("Man-o'-War");
        var vines = C("Vines of Vastwood");

        Hand(P1, manOWar);
        Hand(P2, vines);
        Battlefield(P1, "Island", "Island", "Island");
        Battlefield(P2, force, "Forest");

        Exec(
          At(Step.FirstMain)
            .Cast(manOWar)
            .Target(force),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(force).Zone);
                Equal(Zone.Graveyard, C(vines).Zone);
                Equal(Zone.Battlefield, C(manOWar).Zone);
              })
          );
      }
    }
  }
}