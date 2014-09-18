namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class FleshToDust
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void SacrificeToDestroyArtifact()
            {
                var flesh = C("Flesh To Dust");
                var bear = C("Grizzly Bears");

                Hand(P1, flesh);
                Battlefield(P2, bear);

                Exec(
                  At(Step.FirstMain)
                  .Cast(flesh, target: bear)
                    .Verify(() => { Equal(0, P2.Battlefield.Count); })
                  );
            }
        }
    }
}
