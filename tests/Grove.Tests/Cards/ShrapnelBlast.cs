namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ShrapnelBlast
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastShrapnelBlast()
            {
                Hand(P1, "Shrapnel Blast");
                Battlefield(P1, "Profane Memento", "Mountain", "Mountain", "Mountain", "Mountain");

                P2.Life = 5;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
