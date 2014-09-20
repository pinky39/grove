namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MultanisPresence
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DrawCard()
      {
        var shock = C("Shock");
        var counterspell = C("Counterspell");
        
        Hand(P1, shock);        
        Hand(P2, counterspell);
        Battlefield(P1, "Multani's Presence");


        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2)
            .Cast(counterspell, target: E(shock), stackShouldBeEmpty: false),
          At(Step.SecondMain)
            .Verify(() => Equal(1, P1.Hand.Count))
          );

      }
    }
  }
}