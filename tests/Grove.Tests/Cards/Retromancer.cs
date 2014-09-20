namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Retromancer
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void TargetedBySpell()
      {
        var retromancer = C("Retromancer");
        var bolt = C("Lightning Bolt");

        Hand(P1, bolt);
        Battlefield(P2, retromancer);


        Exec(
          At(Step.FirstMain)
            .Cast(bolt, target: retromancer),
          At(Step.SecondMain)
            .Verify(() => Equal(17, P1.Life))
          );
      }

      [Fact]
      public void TargetedByAbility()
      {
        var retromancer = C("Retromancer");
        var seal = C("Seal of Fire");

        Battlefield(P1, seal);
        Battlefield(P2, retromancer);


        Exec(
          At(Step.FirstMain)
            .Activate(seal, target: retromancer),
          At(Step.SecondMain)
            .Verify(() => Equal(17, P1.Life))
          );
      }
    }
  }
}