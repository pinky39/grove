namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Infrastructure;
  using Xunit;

  public class RootboundCrag
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void ComesIntoPlayTapped()
      {
        var crag = C("Rootbound Crag");

        Hand(P1, crag);
        Battlefield(P1);

        Exec(
          At(Step.FirstMain)
            .Cast(crag)
            .Verify(() => True(C(crag).IsTapped))
          );
      }

      [Fact]
      public void ComesIntoPlayUntapped()
      {
        var crag = C("Rootbound Crag");

        Hand(P1, crag);
        Battlefield(P1, "Forest");

        Exec(
          At(Step.FirstMain)
            .Cast(C(crag))
            .Verify(() => False(C(crag).IsTapped))
          );
      }
    }
  }
}