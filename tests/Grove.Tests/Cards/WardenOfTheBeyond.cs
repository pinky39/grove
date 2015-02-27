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
        Battlefield(P1, warden);
        Battlefield(P2, wall);

        Exec(
          At(Step.FirstMain)
            .Cast(spell, target: wall)
            .Verify(() =>
            {              
              Equal(1, P2.Exile.Count());
              Equal(4, C(warden).Power);
            })
          );
      }
    }
  }
}
