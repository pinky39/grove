namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Exhaustion
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void LandsCreaturesDoNotUntap()
      {
        var exhaustion = C("Exhaustion");
        var bear = C("Grizzly Bears");
        var forest = C("Forest");


        Battlefield(P2, bear.Tap(), forest.Tap());
        Hand(P1, exhaustion);

        Exec(
          At(Step.FirstMain)
            .Cast(exhaustion),
          At(Step.FirstMain, turn: 2)
            .Verify(() =>
              {
                True(C(forest).IsTapped);
                True(C(bear).IsTapped);
              }),
          At(Step.FirstMain, turn: 4)
            .Verify(() =>
              {
                False(C(forest).IsTapped);
                False(C(bear).IsTapped);
              })
          );
      }
    }
  }
}