namespace Grove.Tests.Cards
{
  using Core;
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class LiltingRefrain
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void CounterDragon()
      {
        var dragon = C("Shivan Dragon");
        
        Hand(P1, dragon);        
        Battlefield(P2, "Lilting Refrain");
        
        Exec(
          At(Step.FirstMain, 3)
            .Cast(dragon)
            .Verify(() => Equal(Zone.Graveyard, C(dragon).Zone))
        );
      }
    }
  }
}