namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class PillarOfLight
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void Exile()
            {
                Hand(P1, "Pillar of Light");
                Battlefield(P1, "Kinsbaile Skirmisher", "Plains", "Plains", "Plains");

                P2.Life = 2;
                Battlefield(P2, "Baneslayer Angel");

                RunGame(1);

                Equal(0, P2.Life);
                Equal(1, P2.Exile.Count());
            }
        }
    }
}
