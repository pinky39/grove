namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BurstLightning
  {
    public class Predefined : PredefinedScenario
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
            .Cast(burst, target: armadon, index: 1)
            .Verify(() => Equal(Zone.Graveyard, C(armadon).Zone))
          );
      }
    }
  }
}