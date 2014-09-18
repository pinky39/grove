namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class PeelFromReality
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void ReturnCreatures()
            {
                P1.Life = 2;
                Hand(P1, "Peel from Reality");
                Battlefield(P1, "Island", "Island", "Island", "Island", "Grizzly Bears");

                Battlefield(P2, "Blood Host");

                RunGame(2);

                Equal(0, P1.Battlefield.Creatures.Count());
                Equal(2, P1.Life);
            }
        }
    }
}
