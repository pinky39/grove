namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Remembrance
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void FetchBear()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");
        var shock = C("Shock");

        Hand(P1, shock);
        Battlefield(P2, bear1, "Remembrance", "Forest", "Plains");
        Library(P2, bear2, "Verdant Force");

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: bear1),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(bear1).Zone);
                Equal(Zone.Hand, C(bear2).Zone);
              })
          );
      }
    }
  }
}