namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class BurstLightning : PredifinedScenario
  {
    public class Predefined : PredifinedScenario
    {
      [Fact]
      public void Deals4DamageWithKicker()
      {
        var burst = C("Burst Lightning");
        var armadon = C("Trained Armodon");

        Hand(P1, burst);
        Battlefield(P2, armadon);

        Exec(
          At(Step.FirstMain)
            .Cast(burst, target: armadon, payKicker: true)
            .Verify(() => Equal(Zone.Graveyard, C(armadon).Zone))
          );
      }
    }
  }
}