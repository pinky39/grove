namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class SiegeDragon
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastSiegeDragon()
            {
                Hand(P1, "Siege Dragon");
                Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");

                Battlefield(P2, "Wall of Fire");

                RunGame(1);

                Equal(0, P1.Hand.Count);
                Equal(0, P2.Battlefield.Count);
            }

            [Fact]
            public void AttackWithSiegeDragon()
            {
                Battlefield(P1, "Siege Dragon");

                Battlefield(P2, "Grizzly Bears");

                RunGame(1);

                Equal(15, P2.Life);
                Equal(0, P2.Battlefield.Count);
            }
        }
    }
}
