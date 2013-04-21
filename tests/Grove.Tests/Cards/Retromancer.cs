namespace Grove.Tests.Cards
{
  using Core;
  using Infrastructure;
  using Xunit;

  public class Retromancer
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Deal3Damage()
      {
        var retromancer = C("Retromancer");
        var bolt = C("Lightning Bolt");
        
        Hand(P1, bolt);
        Battlefield(P2, retromancer);
        

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, target: retromancer),
          At(Step.SecondMain)
            .Verify(() => Equal(17, P1.Life))
          );
      }
    }
  }
}