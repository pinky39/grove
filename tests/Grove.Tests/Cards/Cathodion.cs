namespace Grove.Tests.Cards
{
  using Core;
  using Infrastructure;
  using Xunit;

  public class Cathodion
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Add3ToManaPool()
      {
        var bolt = C("Lightning Bolt");
        var cathodion = C("Cathodion");
        
        Battlefield(P1, cathodion);        
        Hand(P2, bolt);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, target: cathodion)
            .Verify(() =>
              {
                True(P1.HasMana(3));
              })
          );
      }
    }
  }
}