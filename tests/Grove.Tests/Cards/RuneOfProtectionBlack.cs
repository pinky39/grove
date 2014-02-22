namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class RuneOfProtectionBlack
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PreventCombatDamage()
      {
        var dread = C("Dread");
        var rune = C("Rune of Protection: Black");

        Battlefield(P1, dread);
        Battlefield(P2, rune, "Plains", "Plains");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dread),
          At(Step.SecondMain)
            .Verify(() => Equal(20, P2.Life))
          );
      }
    }
  }
}