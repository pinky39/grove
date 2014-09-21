namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class StormtideLeviathan
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void Static()
            {
                var forest = C("Forest");
                Hand(P1, "Fugitive Wizard");
                Battlefield(P1, "Stormtide Leviathan", forest, "Grizzly Bears");

                RunGame(1);

                Equal(12, P2.Life);
                Equal(4, P1.Battlefield.Count);
                True(C(forest).Type.Contains("Island"));
            }
        }
    }
}
