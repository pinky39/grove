namespace Grove.Tests.Cards
{
  using Core;
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class EliteArchers
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void KillArmodon()
      {
        var armodon = C("Trained Armodon");

        Battlefield(P1, armodon);
        Battlefield(P2, "Elite Archers");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(armodon),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(armodon).Zone))
        );
      }
    }
  }
}