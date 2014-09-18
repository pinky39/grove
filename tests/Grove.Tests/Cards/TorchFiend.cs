namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class TorchFiend
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void SacrificeToDestroyArtifact()
            {
                var fiend = C("Torch Fiend");
                var cathodion = C("Cathodion");
                
                Battlefield(P1, fiend);
                Battlefield(P2, cathodion);

                Exec(
                  At(Step.FirstMain)
                    .Activate(fiend, target: cathodion)
                    .Verify(() => { Equal(0, P2.Battlefield.Count); })
                  );
            }
        }
    }
}
