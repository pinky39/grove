namespace Grove.Tests.Cards
{
  using Core;
  using Infrastructure;
  using Xunit;

  public class ClawsOfGix
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SacInResponse()
      {
        var shock = C("Shock");
        var bear = C("Grizzly Bears");
        
        Battlefield(P2, "Claws of Gix", bear, "Mountain");        
        Hand(P1, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: bear)          
            .Verify(() => Equal(21, P2.Life))
          );
      }
    }
  }
}