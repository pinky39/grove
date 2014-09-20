namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ClawsOfGix
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SacInResponse()
      {
        var shock = C("Shock");
        var bear = C("Grizzly Bears");

        Battlefield(P2, "Claws of Gix", bear, "Mountain");
        Hand(P1, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: bear)
            .Verify(() => Equal(21, P2.Life))
          );
      }

      [Fact]
      public void SacLandWhenLifeIsLow()
      {
        var shock = C("Shock");

        Hand(P1, shock);
        Battlefield(P1, "Mountain");

        Battlefield(P2, "Claws of Gix", "Mountain");
        P2.Life = 2;

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2)
            .Verify(() => { Equal(1, P2.Life); })
          );
      }
    }
  }
}