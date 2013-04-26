namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class EnergyField
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PreventDamage()
      {
        var bear = C("Grizzly Bears");
        var shock = C("Shock");

        Battlefield(P2, "Energy Field");

        Hand(P1, shock);
        Battlefield(P1, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2),
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear),
          At(Step.SecondMain)
            .Verify(() => Equal(P2.Life, 20))
          );
      }

      [Fact]
      public void SacrificeField()
      {
        var field = C("Energy Field");
        var shock = C("Shock");

        Battlefield(P1, field);
        Hand(P1, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(field).Zone);
                Equal(18, P2.Life);
              })
          );
      }
    }
  }
}