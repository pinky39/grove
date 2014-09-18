namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Plummet
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void DestroyCreatureWithFlying()
            {
                P1.Life = 6;
                Hand(P1, "Plummet");
                Battlefield(P1, "Forest", "Forest");

                Battlefield(P2, "Kapsho Kitefins", "Blood Host");

                RunGame(2);

                Equal(3, P1.Life);
            }
        }
    }
}
