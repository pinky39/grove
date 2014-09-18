namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class KapshoKitefins
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void EnterSelfOnBattlefield()
            {
                Hand(P1, "Kapsho Kitefins");
                Battlefield(P1, "Grizzly Bears", "Island", "Island", "Island", "Island", "Island", "Island");

                P2.Life = 2;
                Battlefield(P2, "Grizzly Bears", "Island");

                RunGame(1);

                Equal(0, P2.Life);
            }

            [Fact]
            public void EnterAnotherOnBattlefield()
            {
                Hand(P1, "Grizzly Bears");
                Battlefield(P1, "Kapsho Kitefins", "Forest", "Forest");

                P2.Life = 3;
                Battlefield(P2, "Kapsho Kitefins");

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
