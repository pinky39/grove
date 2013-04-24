namespace Grove.Tests.Cards
{
  using Core;
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class Brand
  {
    public class Ai : PredefinedAiScenario
    {
      [Fact]
      public void GainControl()
      {
        var confiscate = C("Confiscate");
        var dragon = C("Shivan Dragon");

        Hand(P1, confiscate);
        Hand(P2, "Brand");
        Battlefield(P2, dragon, "Mountain");

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, target: dragon)
            .Verify(() =>
              {
                Equal(P1, C(dragon).Controller);
                Equal(P2, C(dragon).Owner);
              }),
          At(Step.FirstMain, turn: 2)
            .Verify(() => Equal(P2, C(dragon).Controller))
          );
      }
    }
  }
}