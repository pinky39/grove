namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CovetousDragon
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void SacWhenLastArtifactIsDestroyed()
      {
        var dragon = C("Covetous Dragon");
        var disenchant = C("Disenchant");
        var dragonBlood = C("Dragon Blood");

        Battlefield(P1, dragonBlood, dragon);
        Hand(P2, disenchant);

        Exec(
          At(Step.FirstMain)
            .Cast(disenchant, target: dragonBlood)
            .Verify(() => Equal(Zone.Graveyard, C(dragon).Zone))
          );
      }
    }
  }
}