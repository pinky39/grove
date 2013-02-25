namespace Grove.Tests.Cards
{
  using System.Linq;
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class CruelEdict
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KavuGetsSmashed()
      {
        var edict = C("Cruel Edict");
        var kavu = C("Flametongue Kavu");

        Hand(P1, edict);
        Battlefield(P1, "Swamp", "Swamp");
        Battlefield(P2, kavu);

        RunGame(maxTurnCount: 2);

        Equal(Zone.Graveyard, C(edict).Zone);
        Equal(Zone.Graveyard, C(kavu).Zone);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void TargetOpponentSacrificesCreature()
      {
        var edict = C("Cruel Edict");
        var bear = C("Grizzly Bears");

        Hand(P1, edict);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(edict)
            .Verify(() =>
              Equal(0, P2.Battlefield.Count())
            ));
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DoNotSackTitan()
      {
        var titan = C("Grave Titan");
        var edict = C("Cruel Edict");

        Hand(P2, titan);
        Hand(P1, edict);
        Battlefield(P2, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");


        Exec(
          At(Step.FirstMain, turn: 3)
            .Cast(edict),
          At(Step.SecondMain, turn: 3)
            .Verify(() => Equal(Zone.Battlefield, C(titan).Zone))
          );
      }
    }
  }
}