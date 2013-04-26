namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class GraspOfDarkness
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void CardWithZeroToughnessGoesToGraveyard()
      {
        var elves = C("Llanowar Elves");
        var grasp = C("Grasp of Darkness");

        Battlefield(P2, elves);
        Hand(P1, grasp);

        Exec(
          At(Step.FirstMain)
            .Cast(grasp, target: elves)
            .Verify(() =>
              Equal(0, P2.Battlefield.Count()))
          );
      }
    }
  }
}