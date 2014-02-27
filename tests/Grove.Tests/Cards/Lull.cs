namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Lull
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PreventCombatDamage()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");
        var bear3 = C("Grizzly Bears");
        var lull = C("Lull");

        Hand(P2, lull);
        Battlefield(P1, bear1, bear2);
        Battlefield(P2, bear3);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear1, bear2),
          At(Step.DeclareBlockers)
            .DeclareBlockers(bear1, bear3)
            .Cast(lull),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Battlefield, C(bear1).Zone);
                Equal(Zone.Battlefield, C(bear2).Zone);
                Equal(Zone.Battlefield, C(bear3).Zone);
              })
          );
      }
    }
  }
}