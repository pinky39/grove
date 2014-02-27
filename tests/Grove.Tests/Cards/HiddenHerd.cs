namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HiddenHerd
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Becomes33Beast()
      {
        var herd = C("Hidden Herd");
        var ravine = C("Raging Ravine");

        Battlefield(P2, herd);
        Hand(P1, ravine);

        Exec(
          At(Step.FirstMain)
            .Cast(ravine)
            .Verify(() => { Equal(3, C(herd).Power); })
          );
      }
    }
  }
}