namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class SilentAttendant
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void ActivateEot()
      {
        var attendant = C("Silent Attendant");

        Battlefield(P2, attendant);

        Exec(
          At(Step.FirstMain, 2)
            .Verify(() => Equal(21, P2.Life))
          );
      }
    }
  }
}