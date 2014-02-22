namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class ManaLeak
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Counter()
      {
        var manaLeak = C("Mana Leak");
        var force = C("Verdant Force");

        Hand(P2, manaLeak);
        Hand(P1, force);

        Exec(
          At(Step.FirstMain)
            .Cast(force)
            .Cast(manaLeak, target: E(force), stackShouldBeEmpty: false)
            .Verify(() =>
              {
                Equal(1, P1.Graveyard.Count());
                Equal(1, P2.Graveyard.Count());
              })
          );
      }
    }

    public class PredifinedAi : PredefinedAiScenario
    {
      [Fact]
      public void Counter()
      {
        var manaLeak = C("Mana Leak");
        var force = C("Verdant Force");

        Battlefield(P2, "Island", "Island");
        Hand(P2, manaLeak);
        Hand(P1, force);

        Exec(
          At(Step.FirstMain)
            .Cast(force)
            .Verify(() =>
              {
                Equal(1, P1.Graveyard.Count());
                Equal(1, P2.Graveyard.Count());
              })
          );
      }

      [Fact]
      public void CannotCounter()
      {
        var manaLeak = C("Mana Leak");
        var thrun = C("Thrun, the Last Troll");

        Battlefield(P2, "Island", "Island");
        Hand(P2, manaLeak);
        Hand(P1, thrun);

        Exec(
          At(Step.FirstMain)
            .Cast(thrun)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(thrun).Zone);
                Equal(Zone.Hand, C(manaLeak).Zone);
              })
          );
      }
    }
  }
}