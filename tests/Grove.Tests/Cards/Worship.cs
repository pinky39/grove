namespace Grove.Tests.Cards
{
  using Core;
  using Infrastructure;
  using Xunit;

  public class Worship
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PreventDamage()
      {
        var bear = C("Grizzly Bears");
        var dread = C("Dread");
        var worship = C("Worship");

        P2.Life = 6;
        
        Battlefield(P1, dread);
        Battlefield(P2, bear, worship);
        
        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dread),
          At(Step.SecondMain)
            .Verify(() => Equal(1, P2.Life))        
          );
      }
    }
  }
}