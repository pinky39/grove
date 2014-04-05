namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PhyrexianNegator
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Sacrifice3Permanents()
      {
        var negator = C("Phyrexian Negator");
        var bolt = C("Lightning Bolt");

        Battlefield(P1, negator, "Swamp", "Swamp");
        Hand(P2, bolt);

        Exec(
          At(Step.FirstMain)
           .Cast(bolt, target: negator),
          At(Step.SecondMain)
          .Verify(() => Equal(0, P1.Battlefield.Count)));
      }
    }
  }
}