namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class GlacialCrasher
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void CastGlacialCrasher()
            {
                var destroyLand = C("Lay Waste");
                var creature = C("Glacial Crasher");
                var land = C("Mountain");

                Hand(P1, creature, destroyLand);
                Battlefield(P1, land);

                Exec(
                  At(Step.FirstMain)
                    .Cast(creature)
                    .Verify(() => { False(C(creature).Has().CannotAttack); })
                    .Cast(destroyLand, target: land)
                    .Verify(() => { True(C(creature).Has().CannotAttack); })
                  );
            }
        }
    }
}
