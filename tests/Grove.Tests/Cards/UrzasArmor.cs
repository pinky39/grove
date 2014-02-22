namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class UrzasArmor
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void Prevent1CombatDamage()
      {
        var bear = C("Grizzly Bears");
        var armor = C("Urza's armor");

        Battlefield(P1, bear);
        Battlefield(P2, armor);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear),
          At(Step.SecondMain)
            .Verify(() => Equal(19, P2.Life))
          );
      }
    }
  }
}