namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class BodySnatcher
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void ExileBodySnatcher()
      {
        var snatcher = C("Body Snatcher");

        Hand(P1, snatcher);

        Exec(
          At(Step.FirstMain)
            .Cast(snatcher),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Exile, C(snatcher).Zone))
        );
      }

      [Fact]
      public void PutForceToBattlefield()
      {
        var snatcher = C("Body Snatcher");
        var force = C("Verdant Force");
        var shock = C("Shock");

        Hand(P1, snatcher, force);
        Hand(P2, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(snatcher),
          At(Step.DeclareAttackers)
            .Cast(shock, target: snatcher)
            .Target(force),            
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Exile, C(snatcher).Zone);
                Equal(Zone.Battlefield, C(force).Zone);
              })
        );
      }
    }
  }
}