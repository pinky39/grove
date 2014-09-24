namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class ScrapyardMongrel
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void CastArtifact()
            {
                var mongrel = C("Scrapyard Mongrel");
                var artifact1 = C("Profane Memento");
                var artifact2 = C("Profane Memento");

                Hand(P1, artifact1, artifact2);
                Battlefield(P1, mongrel);
                P2.Life = 5;

                Exec(
                  At(Step.FirstMain)
                    .Cast(artifact1)
                    .Cast(artifact2)
                    .Verify(() =>
                    {
                        True(C(mongrel).Has().Trample);
                        Equal(5, C(mongrel).Power);
                    })
                  );
            }
        }
    }
}
