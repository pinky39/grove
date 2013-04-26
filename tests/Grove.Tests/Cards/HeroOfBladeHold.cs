namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class HeroOfBladeHold
  {
    public class Prefefined : PredefinedScenario
    {
      [Fact]
      public void Attack()
      {
        var hero = C("Hero of Bladehold");

        Battlefield(P1, hero);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(hero),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(3, P1.Battlefield.Creatures.Count());
                Equal(13, P2.Life);
              })
          );
      }
    }
  }
}