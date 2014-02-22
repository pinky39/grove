namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class NantukoShade
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PumpShade()
      {
        var shock = C("Shock");
        var shade = C("Nantuko Shade");

        Battlefield(P2, shade, "Swamp", "Swamp");
        Hand(P1, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, shade)
            .Verify(() =>
              {
                Equal(3, C(shade).Toughness);
                Equal(2, C(shade).Damage);
              })
          );
      }
    }
  }
}