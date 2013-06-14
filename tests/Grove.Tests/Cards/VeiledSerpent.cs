namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class VeiledSerpent
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void CanAttack()
      {
        var shock = C("Shock");
        
        Hand(P1, shock);
        Battlefield(P1, "Island");
        Battlefield(P2, "Veiled Serpent");

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2),
          At(Step.SecondMain, turn: 2)
            .Verify(() => Equal(16, P1.Life))
          );
      }

      [Fact]
      public void CantAttack()
      {
        var shock = C("Shock");

        Hand(P1, shock);        
        Battlefield(P2, "Veiled Serpent");

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2),
          At(Step.SecondMain, turn: 2)
            .Verify(() => Equal(20, P1.Life))
          );
      }
    }
  }
}