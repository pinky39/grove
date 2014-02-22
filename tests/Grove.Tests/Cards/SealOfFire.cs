namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class SealOfFire
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SealTheBear()
      {
        var sealOfFire = C("Seal of Fire");
        var bear = C("Grizzly Bears");

        Battlefield(P1, bear);
        Battlefield(P2, sealOfFire);

        P2.Life = 2;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(2, P2.Life);
                Equal(Zone.Graveyard, C(bear).Zone);
                Equal(Zone.Graveyard, C(sealOfFire).Zone);
              })
          );
      }
    }
  }
}