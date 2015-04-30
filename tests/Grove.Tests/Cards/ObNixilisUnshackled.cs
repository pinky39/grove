namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ObNixilisUnshackled 
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void Loose10LifeSacGranger()
      {
        var granger = C("Yavimaya Granger");
        var ob = C("Ob Nixilis, Unshackled");
        
        Hand(P1, granger);        
        Battlefield(P2, ob);

        Exec(
          At(Step.FirstMain)
            .Cast(granger),
          At(Step.SecondMain)
          .Verify(() =>
            {
              Equal(Zone.Graveyard, C(granger).Zone);
              Equal(10, P1.Life);
              Equal(5, C(ob).Power);
            })
          );

      }
    }
  }
}