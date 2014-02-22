namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class Worship
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void RemovePreventionWhenLeavesBattlefield()
      {
        var disenchant = C("Disenchant");
        var shock = C("Shock");
        var worship = C("Worship");
        var bear = C("Grizzly Bears");

        P1.Life = 1;

        Hand(P2, disenchant, shock);
        Hand(P1, worship);
        Battlefield(P1, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(worship),
          At(Step.SecondMain)
            .Cast(shock, P1)
            .Cast(disenchant, worship, stackShouldBeEmpty: false));

        Equal(-1, P1.Life);
      }

      [Fact]
      public void PreventLifeloss()
      {
        var bear = C("Grizzly Bears");
        var dread = C("Dread");
        var worship = C("Worship");

        P2.Life = 6;

        Battlefield(P1, dread);
        Battlefield(P2, bear, worship);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dread),
          At(Step.SecondMain)
            .Verify(() => Equal(1, P2.Life))
          );
      }
    }
  }
}