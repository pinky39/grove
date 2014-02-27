namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SignInBlood
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void TargetPlayerDrawsCardsAndLoosesLife()
      {
        var sign = C("Sign in Blood");
        Hand(P1, sign);

        Exec(
          At(Step.FirstMain)
            .Cast(sign, target: P2)
            .Verify(() =>
              {
                Equal(2, P2.Hand.Count());
                Equal(18, P2.Life);
              }));
      }
    }
  }
}