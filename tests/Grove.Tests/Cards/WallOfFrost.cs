namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class WallOfFrost
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void BlockWithWall()
            {
                var wall = C("Wall Of Frost");
                Battlefield(P1, "Thran War Machine", "Forest", "Forest", "Forest", "Forest");
                Battlefield(P2, wall);

                RunGame(1);

                Equal(7, C(wall).Toughness);
                Equal(5, P1.Battlefield.Count);                
                True(P1.Battlefield.Creatures.Any(c => c.IsTapped));
            }
        }
    }
}
