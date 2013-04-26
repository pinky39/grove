namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class DayOfJudgment
  {
    public class Predefined : PredefinedAiScenario
    {
      [Fact]
      public void DestroyAllCreatures()
      {
        var day = C("Day of Judgment");

        Battlefield(P1, "Forest", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
        Battlefield(P2, "Forest", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        Hand(P1, day);

        Exec(
          At(Step.FirstMain)
            .Cast(day)
            .Verify(() =>
              {
                Equal(1, P1.Battlefield.Count());
                Equal(1, P2.Battlefield.Count());
              })
          );
      }
    }
  }
}