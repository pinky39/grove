namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class SolemnOffering
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void DestroyArtifact()
            {
                Hand(P1, "Solemn Offering");
                Battlefield(P1, "Plains", "Plains", "Plains");

                Battlefield(P2, "Staff of the Death Magus");

                RunGame(1);

                Equal(24, P1.Life);
                Equal(0, P2.Battlefield.Count);
            }
        }
    }
}
