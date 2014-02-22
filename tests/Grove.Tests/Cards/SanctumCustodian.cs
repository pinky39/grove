namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class SanctumCustodian
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PreventNext2Damage()
      {
        var bear = C("Grizzly Bears");
        var custodian = C("Sanctum Custodian");
        var shock1 = C("Shock");
        var shock2 = C("Shock");

        Hand(P1, shock1, shock2);
        Battlefield(P2, bear, custodian);

        Exec(
          At(Step.FirstMain, 2)
            .Cast(shock1, target: bear)
            .Activate(custodian, target: bear, stackShouldBeEmpty: false)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(bear).Zone);
                Equal(0, C(bear).Damage);
              }),
          At(Step.SecondMain, 2)
            .Cast(shock2, target: bear)
            .Verify(() => Equal(Zone.Graveyard, C(bear).Zone))
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PreventNext2Damage()
      {
        // this test only works with low player life
        // since the knight with higher life
        // because the block strategy is only 1
        // and does not take damage prevention into account

        // TODO speed up AI so at least 2 different block
        //  strategies can be tried 

        var bear = C("Grizzly Bears");
        var custodian = C("Sanctum Custodian");
        var knight = C("White Knight");

        Battlefield(P1, knight);
        Battlefield(P2, custodian, bear);

        P2.Life = 2;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(knight),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(P2.Life, 2);
                Equal(Zone.Battlefield, C(bear).Zone);
                Equal(Zone.Graveyard, C(knight).Zone);
              })
          );
      }
    }
  }
}