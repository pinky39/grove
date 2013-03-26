namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class NoRestForTheWicked
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void Return2Creatures()
      {
        var force1 = C("Verdant Force");
        var force2 = C("Verdant Force");
        var blade1 = C("Doom blade");
        var blade2 = C("Doom blade");
        var rest = C("No Rest for the wicked");
        
        Battlefield(P2, rest, force1, force2);        
        Hand(P1, blade1, blade2);

        Exec(
          At(Step.FirstMain)
            .Cast(blade1, target: force1)
            .Cast(blade2, target: force2),
          At(Step.FirstMain, 2)
          .Verify(() =>
            {
              Equal(Zone.Hand, C(force1).Zone);
              Equal(Zone.Hand, C(force2).Zone);
              Equal(Zone.Graveyard, C(rest).Zone);
            })

          );
      }
    }
  }
}