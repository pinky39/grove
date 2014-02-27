namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HealingSalve
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PreventDamage()
      {
        var shock = C("Shock");
        var bears = C("Grizzly Bears");

        Hand(P1, shock);
        Hand(P2, "Healing Salve");
        Battlefield(P2, "Plains", bears);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: bears),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Battlefield, C(bears).Zone)));
      }

      [Fact]
      public void GainLife()
      {
        var shock = C("Shock");

        Hand(P1, shock);
        Hand(P2, "Healing Salve");
        Battlefield(P2, "Plains");
        P2.Life = 2;

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2),
          At(Step.SecondMain)
            .Verify(() => Equal(3, P2.Life)));
      }
    }
  }
}