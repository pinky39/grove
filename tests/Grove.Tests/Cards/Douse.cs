namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Douse
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void CounterDragon()
      {
        var dragon = C("Shivan Dragon");
        Hand(P1, dragon);
        Battlefield(P2, "Island", "Island", "Douse");

        Exec(
          At(Step.FirstMain)
            .Cast(dragon)
            .Verify(() => Equal(Zone.Graveyard, C(dragon).Zone))
          );
      }
    }
  }
}