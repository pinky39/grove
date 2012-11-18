namespace Grove.Tests.Cards
{
  using Core;
  using Infrastructure;
  using Xunit;

  public class Fecundity
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void CreaturesControllerDrawsCard()
      {
        Battlefield(P1, "Grizzly Bears", "Fecundity");
        Battlefield(P2, "Grizzly Bears");

        var fallout = C("Volcanic Fallout");        
        Hand(P1, fallout);

        Exec(
          At(Step.FirstMain)
            .Cast(fallout)
            .Verify(() =>
              {
                Equal(1, P1.Hand.Count);
                Equal(1, P2.Hand.Count);
              })
        
          );
      }
    }
  }
}