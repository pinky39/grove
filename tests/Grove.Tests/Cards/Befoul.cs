namespace Grove.Tests.Cards
{
  using Core;
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Befoul
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void CannotBeRegenerated()
      {
        var befoul = C("Befoul");
        var troll = C("Albino Troll");
        
        Hand(P1, befoul);
        Battlefield(P2, "Forest", "Forest", troll);

        Exec(
          At(Step.FirstMain)
            .Cast(befoul, target: troll),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(troll).Zone))
          );
      }
    }
  }
}