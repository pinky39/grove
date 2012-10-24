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
  
  public class AbyssalHorror
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void TargetPlayerDiscardsCards()
      {
        var horror = C("Abyssal Horror");

        Hand(P1, horror);
        Hand(P2, "Shock", "Plains");

        Exec(
          At(Step.FirstMain)
            .Cast(horror)
            .Target(P2)
            .Verify(() => Equal(0, P2.Hand.Count))
          );
      }
    }
  }
}