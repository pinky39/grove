namespace Grove.Tests.Cards
{
  using Core;
  using Infrastructure;
  using Xunit;

  public class Retaliation
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PumpBear()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");
        
        Battlefield(P1, bear1, "Retaliation");
        Battlefield(P2, bear2);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear1),
          At(Step.DeclareBlockers)
            .DeclareBlockers(bear1, bear2),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(3, C(bear1).Power);
                Equal(3, C(bear1).Toughness);
              })
          );
      }
    }
  }
}