namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class HornetNest
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void Get3Tokens()
      {
        var hornets = C("Hornet Nest");
        var bolt = C("Lightning Bolt");

        Hand(P1, bolt);
        Battlefield(P2, hornets);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, target: hornets),
          At(Step.SecondMain)
            .Verify(() => Equal(3, P2.Battlefield.Creatures.Count())));

      }
    }
  }
}