namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class HoardingDragon
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastHoardingDragon()
            {
                Hand(P1, "Hoarding Dragon");
                Library(P1, "Profane Memento");
                Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");

                P2.Life = 4;
                Hand(P2, "Flesh to Dust");
                Battlefield(P2, "Swamp", "Swamp", "Mountain", "Mountain", "Mountain");

                RunGame(3);

                // Turn 1: Cast Hoarding Dragon. Profane Memento is exiled
                // Turn 2: Draw card
                // Turn 3: Draw card. Attack with dragon. Opponent casts Flesh to Dust. Artifact is put into Hand. Play artifact.

                Equal(1, P1.Graveyard.Count);
                Equal(1, P1.Battlefield.Lands.Count(c => c.IsTapped));
                Equal(1, P2.Graveyard.Count);
            }
        }
    }
}
