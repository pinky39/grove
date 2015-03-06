namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DiffusionSliver
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void EnoughManaToPayTheExtraCost()
      {
        var blade = C("Doom blade");
        var sliver = C("Diffusion Sliver");

        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
        Hand(P1, blade);
        Battlefield(P2, sliver, "Diffusion Sliver");

        Exec(
          At(Step.FirstMain)
            .Cast(blade, target: sliver),
          At(Step.SecondMain)
            .Verify(() =>
              Equal(Zone.Graveyard, C(sliver).Zone)));
      }

      [Fact]
      public void NotEnoughManaToPayTheExtraCost()
      {
        var blade = C("Doom blade");
        var sliver = C("Diffusion Sliver");

        Battlefield(P1, "Swamp", "Swamp", "Swamp");
        Hand(P1, blade);
        Battlefield(P2, sliver, "Diffusion Sliver");

        Exec(
          At(Step.FirstMain)
            .Cast(blade, target: sliver),
          At(Step.SecondMain)
            .Verify(() =>
              Equal(Zone.Battlefield, C(sliver).Zone)));
      }
    }
  }
}