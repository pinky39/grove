namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Fecundity
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void GreaterGoodDrawCards()
      {
        var greaterGood = C("Greater Good");
        var engine = C("Wurmcoil Engine");
        Battlefield(P1, greaterGood, "Fecundity", engine);

        Exec(
          At(Step.FirstMain)
            .Activate(greaterGood, costTarget: engine),
          At(Step.SecondMain)
            .Verify(() => { Equal(4, P1.Hand.Count); })
          );
      }

      [Fact]
      public void CreaturesControllerDrawsCard()
      {
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Fecundity");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        var fallout = C("Volcanic Fallout");
        Hand(P1, fallout);

        Exec(
          At(Step.FirstMain)
            .Cast(fallout)
            .Verify(() =>
              {
                Equal(2, P1.Hand.Count);
                Equal(2, P2.Hand.Count);
              })
          );
      }
    }
  }
}