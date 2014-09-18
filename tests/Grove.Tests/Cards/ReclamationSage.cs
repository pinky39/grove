namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ReclamationSage
    {
        public class Predefined : AiScenario
        {
            [Fact]
            public void DestroyTargetArtifact()
            {
                Hand(P1, "Reclamation Sage");
                Battlefield(P1, "Forest", "Forest", "Forest");

                Battlefield(P2, "Staff of the Death Magus");

                RunGame(1);

                Equal(4, P1.Battlefield.Count);
                Equal(0, P2.Battlefield.Count);
            }
        }
    }
}
