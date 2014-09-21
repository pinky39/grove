namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class WallOfLimbs
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void AddCounter()
            {
                var wall = C("Wall of Limbs");
                var soulmender = C("Soulmender");
                Battlefield(P1, soulmender, wall, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
                
                P2.Life = 1;
                Battlefield(P2, "Grizzly Bears");

                RunGame(1);

                Equal(true, C(soulmender).IsTapped);;
                Equal(0, P2.Life);
                Equal(1, P1.Battlefield.Creatures.Count());
//                Equal(1, C(wall).Power);
//                Equal(4, C(wall).Toughness);
            }
        }
    }
}
