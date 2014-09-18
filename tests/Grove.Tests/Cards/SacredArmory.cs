namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class SacredArmory
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void PumpWithSacredArmory()
            {
                Battlefield(P1, "Grizzly Bears", "Sacred Armory", "Mountain", "Mountain", "Mountain", "Mountain");

                P2.Life = 4;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
