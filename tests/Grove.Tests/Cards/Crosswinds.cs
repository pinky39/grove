namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Crosswinds
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void WhenCreatureGainsFlyingCrosswindsIsApplied()
      {
        var bear = C("Grizzly Bears");
        var embrace = C("Shiv's Embrace");

        Hand(P1, embrace);
        Battlefield(P1, bear, "Crosswinds");

        Exec(
          At(Step.FirstMain)
            .Verify(() => Equal(2, C(bear).Power)),
          At(Step.SecondMain)
            .Cast(embrace, target: bear)
            .Verify(() =>
              {
                Equal(4, C(bear).Toughness);
                Equal(2, C(bear).Power);
              })
          );
      }
    }
  }
}