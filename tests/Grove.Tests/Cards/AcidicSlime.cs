namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class AcidicSlime
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DestroyLand()
      {
        var slime = C("Acidic Slime");
        var forest = C("Forest");

        Hand(P1, slime);
        Battlefield(P2, forest);

        Exec(
          At(Step.FirstMain)
            .Cast(slime)
            .Target(forest)
            .Verify(() => Equal(Zone.Graveyard, C(forest).Zone))
          );
      }
    }
  }
}