namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class FeralIncarnation
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastFeralIncarnation()
            {
                Hand(P1, "Feral Incarnation");
                Battlefield(P1, "Forest", "Mountain", "Mountain", "Mountain", "Mountain", "Forest", "Mountain", "Mountain", "Mountain", "Mountain");

                RunGame(1);

                Equal(3, P1.Battlefield.Creatures.Count());
            }
        }
    }
}
