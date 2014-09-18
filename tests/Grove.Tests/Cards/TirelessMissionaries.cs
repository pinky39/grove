namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class TirelessMissionaries
    {
        public class Predefined : AiScenario
        {
            [Fact]
            public void PlayCard()
            {
                Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains");
                Hand(P1, "Tireless Missionaries");

                RunGame(1);

                Equal(23, P1.Life);
            }
        }
    }
}
