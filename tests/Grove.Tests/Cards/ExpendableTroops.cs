namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class ExpendableTroops
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void KillSomnophore()
      {
        var somnophore = C("Somnophore");
        var troops = C("Expendable Troops");
        
        Battlefield(P1, somnophore);        
        Battlefield(P2, troops);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(somnophore),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(somnophore).Zone);
                Equal(Zone.Graveyard, C(troops).Zone);
              })
          );
      }
    }
  }
}