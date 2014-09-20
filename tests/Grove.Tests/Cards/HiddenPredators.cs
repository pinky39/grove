namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HiddenPredators
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void CreatureWithPower4IsOnBattlefield()
      {
        var predators = C("Hidden Predators");

        Hand(P1, predators);
        Battlefield(P2, "Llanowar Behemoth");

        Exec(
          At(Step.FirstMain)
            .Cast(predators)
            .Verify(() => Equal(4, C(predators).Power))
          );
      }

      [Fact]
      public void CreatureWithPower4JoinsBattlefield()
      {
        var predators = C("Hidden Predators");
        var behemoth = C("Llanowar Behemoth");

        Hand(P1, behemoth);
        Battlefield(P2, predators);

        Exec(
          At(Step.FirstMain)
            .Cast(behemoth)
            .Verify(() => Equal(4, C(predators).Power))
          );
      }

      [Fact]
      public void CreatureGetsABoost()
      {
        var predators = C("Hidden Predators");
        var bear = C("Grizzly Bears");
        var embrace = C("Gaea's Embrace");

        Hand(P1, embrace);
        Battlefield(P1, bear);
        Battlefield(P2, predators);

        Exec(
          At(Step.FirstMain)
            .Cast(embrace, target: bear)
            .Verify(() => Equal(4, C(predators).Power))
          );
      }
    }
  }
}