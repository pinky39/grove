namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class HeliodsPilgrim
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastHeliodsPilgrimSearchAura()
            {
                Library(P1, "Plains","Divine Favor", "Plains");
                Battlefield(P1, "Plains", "Plains", "Forest");
                Hand(P1, "Heliod's Pilgrim");

                RunGame(1);

                True(P1.Hand.Any(c => c.Is().Aura));
            }
        }
    }
}
