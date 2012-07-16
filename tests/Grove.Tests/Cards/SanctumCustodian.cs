namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class SanctumCustodian
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PreventNext2Damage()
      {
        var bear = C("Grizzly Bears");
        var custodian = C("Sanctum Custodian");
        var shock1 = C("Shock");
        var shock2 = C("Shock");

        Hand(P1, shock1, shock2);
        Battlefield(P2, bear, custodian);

        Exec(
          At(Step.FirstMain, 2)
            .Cast(shock1, target: bear)
            .Activate(custodian, target: bear)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(bear).Zone);
                Equal(0, C(bear).Damage);
              }),
          At(Step.SecondMain, 2)
            .Cast(shock2, target: bear)
            .Verify(() => Equal(Zone.Graveyard, C(bear).Zone))
          );
        
      }
    }
  }
}