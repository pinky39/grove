namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class GargoyleSentinel
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void GainsFlying()
            {
                Battlefield(P1, "Gargoyle Sentinel", "Swamp", "Swamp", "Swamp");

                P2.Life = 3;
                Battlefield(P2, "Blood Host");

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
