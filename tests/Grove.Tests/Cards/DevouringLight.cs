namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class DevouringLight
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastDevouringLight()
            {
                Battlefield(P1, "Grizzly Bears", "Grizzly Bears");

                P2.Life = 2;
                Battlefield(P2, "Grizzly Bears", "Plains", "Plains");
                Hand(P2, "Devouring Light");

                RunGame(1);

                Equal(2, P2.Life);
                Equal(0, P2.Hand.Count);
                Equal(2, P2.Battlefield.Count);
            }
        }
    }
}
