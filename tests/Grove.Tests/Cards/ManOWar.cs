namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.States;
  using Gameplay.Zones;
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
      public void BounceSelf()
      {
        var engine = C("Wurmcoil Engine");
        var manOWar = C("Man-o'-War");
        var vines = C("Vines of Vastwood");

        Hand(P1, vines);
        Battlefield(P1, engine, "Forest");
        Battlefield(P2, "Island", "Island", "Island");
        Hand(P2, manOWar);

        Exec(
          At(Step.FirstMain, turn: 2)
            .Cast(vines, target: engine, condition: (g) => { return g.Stack.HasSpellWithName("Man-o'-War"); }),
          At(Step.SecondMain, turn: 2)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(engine).Zone);
                Equal(Zone.Graveyard, C(vines).Zone);
                Equal(Zone.Hand, C(manOWar).Zone);
                Equal(3, P2.Battlefield.Lands.Count(c => c.IsTapped));
              })
          );
      }

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