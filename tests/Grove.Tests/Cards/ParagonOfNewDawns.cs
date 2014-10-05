namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ParagonOfNewDawns
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PumpYourCreatures()
      {
        var soldier = C("Paragon Of New Dawns");
        var bear = C("Grizzly Bears");
        var cat1 = C("Oreskos Swiftclaw");
        var cat2 = C("Oreskos Swiftclaw");

        Hand(P1, soldier);
        Battlefield(P1, bear, cat1);
        Battlefield(P2, cat2);

        Exec(
          At(Step.FirstMain)
            .Cast(soldier)
            .Verify(() =>
            {
              Equal(2, C(bear).Power);
              Equal(2, C(soldier).Power);
              Equal(4, C(cat1).Power);
              Equal(3, C(cat2).Power);              
            })
          );
      }
    }
  }
}
