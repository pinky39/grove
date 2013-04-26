namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Pestilence
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DealDamageBeforeDestroyed()
      {
        var bear = C("Grizzly Bears");
        var pestilence = C("Pestilence");
        var disenchant = C("Disenchant");

        Hand(P1, disenchant);
        Battlefield(P1, bear);
        Battlefield(P2, "Swamp", "Swamp", pestilence);

        Exec(
          At(Step.FirstMain)
            .Cast(disenchant, target: pestilence)
            .Verify(() => Equal(Zone.Graveyard, C(bear).Zone))
          );
      }
    }
  }
}