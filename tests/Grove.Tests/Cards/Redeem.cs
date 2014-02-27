namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Redeem
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PreventDamageToBears()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");

        Battlefield(P1, bear1, bear2);

        Hand(P2, "Redeem");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Plains", "Forest");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear1, bear2),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(0, P1.Battlefield.Count);
                Equal(4, P2.Battlefield.Count);
              })
          );
      }
    }
  }
}