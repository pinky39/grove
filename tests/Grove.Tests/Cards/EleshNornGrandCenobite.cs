namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class EleshNornGrandCenobite
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PumpYourCreaturesDestroyOpponents()
      {
        var elesh = C("Elesh Norn, Grand Cenobite");
        var bear = C("Grizzly Bears");

        Hand(P1, elesh);
        Battlefield(P1, bear, "Grizzly Bears", "Grizzly Bears");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Cast(elesh)
            .Verify(() =>
              {
                Equal(4, P1.Battlefield.Creatures.Count());
                Equal(4, C(bear).Power);
                Equal(0, P2.Battlefield.Creatures.Count());
              })
          );
      }
    }
  }
}