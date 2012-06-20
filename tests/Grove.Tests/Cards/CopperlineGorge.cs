namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Infrastructure;
  using Xunit;

  public class CopperlineGorge
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void ComesIntoPlayTapped()
      {
        var gorge = C("Copperline Gorge");

        Hand(P1, gorge);
        Battlefield(P1, "Forest", "Forest", "Forest");

        Exec(
          At(Step.FirstMain)
            .Cast(gorge)
            .Verify(() => True(C(gorge).IsTapped))
          );
      }

      [Fact]
      public void ComesIntoPlayUntapped()
      {
        var gorge = C("Copperline Gorge");

        Hand(P1, gorge);
        Battlefield(P1, "Forest", "Forest");

        Exec(
          At(Step.FirstMain)
            .Cast(gorge)
            .Verify(() => False(C(gorge).IsTapped))
          );
      }
    }
  }
}