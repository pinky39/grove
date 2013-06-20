namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Vebulid
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void AttackAndDestroy()
      {
        var vebuild = C("Vebulid");

        Hand(P1, vebuild);

        Exec(
          At(Step.FirstMain)
            .Cast(vebuild),
          At(Step.DeclareAttackers, turn: 3)
            .DeclareAttackers(vebuild),
          At(Step.SecondMain, turn: 3)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(vebuild).Zone);
                Equal(18, P2.Life);
              })
          );
      }
    }
  }
}