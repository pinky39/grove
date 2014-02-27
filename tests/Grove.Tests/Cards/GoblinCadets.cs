namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GoblinCadets
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void OpponentGainsControllerOfCadets()
      {
        var cadets = C("Goblin Cadets");
        var bear = C("Grizzly Bears");

        Battlefield(P1, bear);
        Battlefield(P2, cadets);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(cadets),
          At(Step.SecondMain)
            .Verify(() => Equal(P2, C(cadets).Controller))
          );
      }
    }
  }
}