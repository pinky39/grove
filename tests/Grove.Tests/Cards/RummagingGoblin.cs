namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class RummagingGoblin
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void DiscardIsland()
            {
                var goblin = C("Rummaging Goblin");
                var land = C("Island");

                Hand(P1, land);
                Battlefield(P1, goblin);

                Exec(
                  At(Step.FirstMain)
                    .Activate(goblin, costTarget: land)
                    .Verify(() =>
                    {
                        True(C(goblin).IsTapped);
                        Equal(1, P1.Hand.Count);
                        Equal(1, P1.Graveyard.Count);
                    })
                  );
            }
        }
    }
}
