namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class Confiscate
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void ConfiscateCreature()
      {
        var bear = C("Grizzly Bears");
        var confiscate = C("Confiscate");
        var disenchant = C("Disenchant");

        Hand(P1, confiscate);
        Hand(P2, disenchant);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, target: bear),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(P1, C(bear).Controller);
                True(C(bear).HasSummoningSickness);
              }),
          At(Step.EndOfTurn)
            .Cast(disenchant, confiscate)
            .Verify(() => Equal(P2, C(bear).Controller))
          );
      }
    }
  }
}