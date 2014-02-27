namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CaveTiger
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Gets11()
      {
        var bear = C("Grizzly Bears");
        var tiger = C("Cave Tiger");


        Battlefield(P1, tiger);
        Battlefield(P2, bear);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(tiger),
          At(Step.DeclareBlockers)
            .DeclareBlockers(tiger, bear),
          At(Step.SecondMain)
            .Verify(() => Equal(3, C(tiger).Power))
          );
      }
    }
  }
}