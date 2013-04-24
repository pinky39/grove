namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Grove.Core;
  using Infrastructure;
  using Xunit;

  public class RumblingSlum
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Deals1DamageDuringYourUpkeep()
      {
        Battlefield(P1, "Rumbling Slum");

        Exec(
          At(Step.FirstMain, turn: 3)
            .Verify(() => {
              Equal(18, P1.Life);
              Equal(18, P2.Life);
            })
          );
      }
    }
  }
}