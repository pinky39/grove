namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class VeiledSentry
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Becomes11Creature()
      {
        var sentry = C("Veiled Sentry");
        var shock = C("Shock");
        
        Battlefield(P1, sentry);
        Hand(P2, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P1)
            .Verify(() =>
              {
                Equal(1, C(sentry).Power);
                Equal(1, C(sentry).Toughness);
              })
          );
      }
    }
  }
}