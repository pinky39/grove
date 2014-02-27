namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class DoomBlade
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DestroyCreature()
      {
        var blade = C("Doom Blade");
        var bear = C("Grizzly Bears");

        Hand(P1, blade);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(blade, target: bear)
            .Verify(() =>
              {
                Equal(0, P2.Battlefield.Count());
                Equal(1, P2.Graveyard.Count());
              }));
      }
    }
  }
}