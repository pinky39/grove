namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DarkstyleGargoyle
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DoomBladeCannotDestroyIt()
      {
        var gargoyle = C("Darksteel Gargoyle");
        var blade = C("Doom blade");

        Hand(P1, blade);
        Battlefield(P2, gargoyle);

        Exec(
          At(Step.FirstMain)
            .Cast(blade, gargoyle)
            .Verify(() => Equal(Zone.Battlefield, C(gargoyle).Zone))
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void BlockDeathtouch()
      {
        var gargoyle = C("Darksteel Gargoyle");
        var engine = C("Wurmcoil Engine");

        Battlefield(P2, gargoyle);
        Battlefield(P1, engine);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(engine),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Battlefield, C(gargoyle).Zone);
              }));
      }
    }
  }
}