namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class PitTrap
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DestroyForce()
      {
        var force = C("Verdant Force");

        Battlefield(P1, force);
        Battlefield(P2, "Pit Trap", "Swamp" ,"Swamp");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(force),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(force).Zone))
          );
      }
    }
  }
}