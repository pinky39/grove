namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Antagonism
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DealsDamageToBoth()
      {
        var antagonism = C("Antagonism");

        Hand(P1, antagonism);

        Exec(
          At(Step.FirstMain)
            .Cast(antagonism),
          At(Step.FirstMain, turn: 4)
            .Verify(() =>
              {
                Equal(16, P1.Life);
                Equal(18, P2.Life);
              })
          );
      }

      [Fact]
      public void DealsNoDamageToYouAfterShock()
      {
        var antagonism = C("Antagonism");
        var shock = C("Shock");

        Hand(P1, antagonism, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2),
          At(Step.SecondMain)
            .Cast(antagonism),
          At(Step.FirstMain, turn: 3)
            .Verify(() =>
              {
                Equal(20, P1.Life);
                Equal(16, P2.Life);
              })
          );
      }
    }
  }
}