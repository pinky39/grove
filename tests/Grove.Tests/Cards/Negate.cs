namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Negate
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void Counterspell()
            {
                P1.Life = 2;
                Hand(P1, "Negate");
                Battlefield(P1, "Island", "Island");

                Hand(P2, "Meteorite");
                Battlefield(P2, "Island", "Island", "Island", "Island", "Island", "Island");

                RunGame(2);

                Equal(2, P1.Life);
            }
        }
    }
}
