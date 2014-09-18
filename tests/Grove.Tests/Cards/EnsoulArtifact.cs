namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class EnsoulArtifact
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void EnchantWithEnsoulArtifact()
            {
                var aura = C("Ensoul Artifact");
                var artifact = C("Profane Memento");

                Hand(P1, aura);
                Battlefield(P1, artifact);

                Exec(
                  At(Step.FirstMain)
                  .Cast(aura, target: artifact)
                    .Verify(() => { True(C(artifact).Is().Creature); })
                  );
            }
        }
    }
}
