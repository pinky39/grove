namespace Grove.Tests.Cards
{
  using System.Linq;
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class FlametongueKavu
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BugGameCrashesWhenPlayingKavu()
      {
        Battlefield(P1, "Forest", "Mountain", "Llanowar Elves", "Llanowar Elves", "Forest");
        var kavu = C("Flametongue Kavu");
        Hand(P1, kavu);

        var bear = C("Grizzly Bears");
        Battlefield(P2, "Forest", "Mountain", C("Llanowar Elves").Tap(), "Llanowar Elves", bear);

        RunGame(maxTurnCount: 2);

        Equal(Zone.Battlefield, C(kavu).Zone);
        Equal(Zone.Graveyard, C(bear).Zone);
      }

      [Fact]
      public void KavuEatsTheElephant()
      {
        var kavu = C("Flametongue Kavu");
        Hand(P1, kavu);
        Battlefield(P1, C("Forest"), C("Forest"), C("Forest"), C("Mountain"));
        Battlefield(P2, C("Trained Armodon"), C("Shivan Dragon"));

        RunGame(maxTurnCount: 1);

        Equal(Zone.Battlefield, C(kavu).Zone);
        Equal(1, P2.Battlefield.Count());
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Deals4DamageToTargetCreature()
      {
        var kavu = C("Flametongue Kavu");
        var bear = C("Grizzly Bears");

        Hand(P1, kavu);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(kavu)
            .Target(bear)
            .Verify(() => {
              Equal(1, P2.Graveyard.Count());
              Equal(1, P1.Battlefield.Count());
            })
          );
      }

      [Fact]
      public void KavuTargetIsDestroyed()
      {
        var kavu = C("Flametongue Kavu");
        var bear = C("Grizzly Bears");
        var shock = C("Shock");

        Hand(P1, kavu);
        Hand(P2, shock);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(kavu)
            .Target(bear)
            .Cast(shock, target: bear)
            .Verify(() => {
              Equal(2, P2.Graveyard.Count());
              Equal(0, C(bear).Damage);
              Equal(1, P1.Battlefield.Count());
            })
          );
      }
    }
  }
}