namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class ShivanDragon
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Gets10ForOneRedManaUntilEot()
      {
        var dragon = C("Shivan Dragon");

        Battlefield(P1, dragon);

        Exec(
          At(Step.FirstMain)
            .Activate(dragon)
            .Activate(dragon)
            .Verify(() => Equal(7, C(dragon).Power)),
          At(Step.FirstMain, turn: 2)
            .Verify(() => Equal(5, C(dragon).Power)));
      }

      public class Ai : AiScenario
      {
        [Fact]
        public void ActivateDragonBeforeAttack()
        {
          Battlefield(P1, C("Shivan Dragon"), C("Mountain"), C("Mountain"), C("Mountain"));

          RunGame(maxTurnCount: 2);

          Equal(12, P2.Life);
        }
      }
    }
  }
}