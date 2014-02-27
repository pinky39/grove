namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MotherOfRunes
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackForKill()
      {
        Battlefield(P1, "Mother Of Runes", "Grizzly Bears");
        Battlefield(P2, "Trained Armodon");

        P2.Life = 2;
        RunGame(1);

        Equal(0, P2.Life);
      }
      
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DeflectShock()
      {
        var shock = C("Shock");
        var bears = C("Grizzly Bears");
        
        Hand(P1, shock);        
        Battlefield(P2, "Mother Of Runes", bears);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: bears),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Battlefield, C(bears).Zone))
          );
      }
    }
  }
}