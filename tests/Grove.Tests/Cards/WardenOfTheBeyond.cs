namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class WardenOfTheBeyond
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void ExileCardGive22()
      {
        var warden = C("Warden Of The Beyond");
        var spell = C("Pillar of Light");
        var wall = C("Wall of Frost");

        Hand(P1, spell);
        Battlefield(P1, warden, wall);

        Exec(
          At(Step.FirstMain)
            .Cast(spell, target: wall)
            .Verify(() =>
            {
              Equal(1, P1.Battlefield.Count);
              Equal(1, P1.Exile.Count());
              Equal(4, C(warden).Power);
            })
          );
      }
    }
  }
}
