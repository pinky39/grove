namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class FledglingOsprey
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void OspreyHasFlyingAsLongItsEnchanted()
      {
        var osprey = C("Fledgling Osprey");
        var rancor = C("Rancor");
        var disenchant = C("Disenchant");

        Battlefield(P1, osprey);        
        Hand(P1, rancor, disenchant);

        Exec(
          At(Step.FirstMain)
            .Cast(rancor, target: osprey)
            .Verify(() => True(C(osprey).Has().Flying))
            .Cast(disenchant, rancor)
            .Verify(() => False(C(osprey).Has().Flying))
          );
      }
    }
  }
}