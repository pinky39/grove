namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class AcidicSlime
  {
    public class Predefined : PredifinedScenario
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