namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class Songstitcher
  {
    public class Predefined : PredefinedAiScenario
    {
      [Fact]
      public void PreventDamage()
      {
        var dragon = C("Shivan Dragon");
        var titcher = C("Songstitcher");

        Battlefield(P1, dragon);
        Battlefield(P2, titcher, "Plains", "Plains");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dragon),
          At(Step.DeclareBlockers)
            .Activate(titcher, target: dragon),
          At(Step.SecondMain)
            .Verify(() => Equal(20, P2.Life))
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PreventDamage()
      {
        var dragon = C("Shivan Dragon");
        var titcher = C("Songstitcher");

        Battlefield(P1, dragon);
        Battlefield(P2, titcher, "Plains", "Plains");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dragon),
          At(Step.SecondMain)
            .Verify(() => Equal(20, P2.Life))
          );
      }
    }
  }
}