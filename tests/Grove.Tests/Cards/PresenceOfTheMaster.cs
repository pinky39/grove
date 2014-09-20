namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PresenceOfTheMaster
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void CounterRancor()
      {
        var rancor = C("Rancor");
        var bear = C("Grizzly Bears");

        Hand(P1, rancor);
        Battlefield(P1, "Presence of the Master", bear);

        Exec(
          At(Step.FirstMain)
            .Cast(rancor, target: bear),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(rancor).Zone))
          );
      }
    }
  }
}