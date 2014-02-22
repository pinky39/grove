namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

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