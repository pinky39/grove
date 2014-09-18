namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class KinsbaileSkirmisher
    {
        public class Predefined : AiScenario
        {
            [Fact]
            public void Put11Counter()
            {
                Battlefield(P1, "Grizzly Bears", "Plains", "Plains");
                Hand(P1, "Kinsbaile Skirmisher");
                    
                P2.Life = 3;
                
                RunGame(1);
                
                Equal(2, P1.Battlefield.Creatures.Count());
                Equal(0, P2.Life);            
            }
        }
    }
}
