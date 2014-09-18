namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class InvasiveSpecies
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void ReturnPermament()
            {
                Hand(P1, "Invasive Species");
                Battlefield(P1, "Grizzly Bears", "Forest", "Forest", "Forest");

                Battlefield(P2, "Grizzly Bears");

                RunGame(1);

                Equal(1, P2.Battlefield.Count);
                Equal(4, P1.Battlefield.Count);
            }
        }
    }
}
