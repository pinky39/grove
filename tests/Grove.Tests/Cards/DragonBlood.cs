namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class DragonBlood
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PumpBeforeDestroyed()
      {
        var bear = C("Grizzly Bears");
        var blood = C("Dragon Blood");
        var disenchant = C("Disenchant");

        Hand(P1, disenchant);
        Battlefield(P2, bear, "Plains", "Plains", "Plains", blood);

        Exec(
          At(Step.FirstMain)
            .Cast(disenchant, target: blood)
            .Verify(() => Equal(3, C(bear).Power))
          );
      }

      [Fact]
      public void PumpEot()
      {
        var songstitcher = C("Songstitcher");

        Battlefield(P2, "Plains", "Plains",
          "Dragon Blood", "Plains", songstitcher);

        Exec(
          At(Step.Upkeep, 2)
            .Verify(() => Equal(1, C(songstitcher).Counters))
          );
      }

      public class Ai : AiScenario
      {
        [Fact]
        public void PumpUnblockedCreature()
        {
          var blood = C("Dragon Blood");
          var bear = C("Grizzly Bears");

          Battlefield(P1, blood, bear, "Plains", "Plains", "Plains");
          RunGame(1);
          Equal(17, P2.Life);
        }
      }
    }
  }
}