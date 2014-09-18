namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class FrostLynx
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void TapCreature()
            {
                Hand(P1, "Frost Lynx");
                Battlefield(P1, "Island", "Island", "Island");

                Battlefield(P2, "Blood Host");

                RunGame(3);

                Equal(18, P2.Life);
            }
        }
    }
}
