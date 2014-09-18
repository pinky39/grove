namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class SanctifiedCharge
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastSanctifiedCharge()
            {
                Hand(P1, "Sanctified Charge");
                Battlefield(P1, "Grizzly Bears", "Oreskos Swiftclaw", "Plains", "Mountain", "Mountain", "Mountain", "Mountain", "Forest", "Mountain", "Mountain", "Mountain", "Mountain");

                P2.Life = 9;
                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
