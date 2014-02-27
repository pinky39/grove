namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class TaintedAether
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void SacrificeCreatureOrLand()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");

        Hand(P1, bear1);
        Hand(P2, bear2);

        Battlefield(P1, "Tainted Aether", "Forest", "Grizzly Bears");
        Battlefield(P2, "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Cast(bear1)
            .Verify(() =>
              Equal(1, P1.Battlefield.Creatures.Count())),
          At(Step.FirstMain, turn: 2)
            .Cast(bear2)
            .Verify(() => Equal(1, P2.Battlefield.Creatures.Count())));
      }
    }
  }
}