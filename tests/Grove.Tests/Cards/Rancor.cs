namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Rancor
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void ReturnToOwnerHand()
      {
        var bear = C("Grizzly Bears");
        var rancor = C("Rancor");
        var shock = C("Shock");

        Battlefield(P2, bear);
        Hand(P1, rancor, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(rancor, target: bear),
          At(Step.SecondMain)
            .Cast(shock, target: bear)
            .Verify(() => True(P1.Hand.Any(x => x == C(rancor)))));
      }
    }
  }
}