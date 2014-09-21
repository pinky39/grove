namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class WallOfEssence
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void GetLife()
            {
                var thran = C("Thran War Machine");
                Battlefield(P1, thran, "Forest", "Forest", "Forest", "Forest");
                
                P2.Life = 1;
                Battlefield(P2, "Wall of Essence");

                RunGame(1);

                Equal(5, P2.Life);
                Equal(0, P2.Battlefield.Count);
            }
        }
    }
}
