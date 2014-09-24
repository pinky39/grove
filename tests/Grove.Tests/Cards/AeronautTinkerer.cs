namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class AeronautTinkerer
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void CastAeronautTinkerer()
            {
                var fiend = C("Torch Fiend");
                var tinker = C("Aeronaut Tinkerer");
                var artifact = C("Profane Memento");

                Hand(P1, tinker);
                Battlefield(P1, artifact, fiend);

                Exec(
                  At(Step.FirstMain)
                    .Cast(tinker)
                    .Verify(() => { True(C(tinker).Has().Flying); })
                    .Activate(fiend, target: artifact)
                    .Verify(() => { False(C(tinker).Has().Flying); })
                  );
            }

            [Fact]
            public void CastArtifact()
            {
                var fiend = C("Torch Fiend");
                var tinker = C("Aeronaut Tinkerer");
                var artifact = C("Profane Memento");

                Hand(P1, artifact);
                Battlefield(P1, fiend, tinker);

                Exec(
                  At(Step.FirstMain)
                    .Cast(artifact)
                    .Verify(() => { True(C(tinker).Has().Flying); })
                    .Activate(fiend, target: artifact)
                    .Verify(() => { False(C(tinker).Has().Flying); })
                  );
            }
        }
    }
}
