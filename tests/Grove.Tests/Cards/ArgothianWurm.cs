namespace Grove.Tests.Cards
{
  using System.Linq;
  using Core;
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class ArgothianWurm
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SacLand()
      {
        var wurm = C("Argothian Wurm");

        Hand(P1, wurm);
        Battlefield(P2, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

        Exec(
          At(Step.FirstMain)
            .Cast(wurm)
            .Verify(() =>
              {
                Equal(Zone.Library, C(wurm).Zone);
                Equal(1, P2.Graveyard.Count(x => x.Is().Land));
              }),
          At(Step.FirstMain, 3)
            .Verify(() => Equal(Zone.Hand, C(wurm).Zone))
          );
      }
    }
  }
}