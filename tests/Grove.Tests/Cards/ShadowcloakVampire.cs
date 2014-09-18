namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class ShadowcloakVampire
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void PayLife()
            {
                Battlefield(P1, "Shadowcloak Vampire");
                P1.Life = 10;

                Battlefield(P2, "Grizzly Bears");
                P2.Life = 4;

                RunGame(1);

                Equal(0, P2.Life);
                Equal(8, P1.Life);                
            }
        }
    }
}
