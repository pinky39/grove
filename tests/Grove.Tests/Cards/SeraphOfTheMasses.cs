
namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class SeraphOfTheMasses
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastSeraphOfTheMasses()
            {
                Hand(P1, "Seraph of the Masses");
                Battlefield(P1, "Plains", "Plains", "Mountain", "Mountain", "Mountain", "Mountain", "Grizzly Bears");
                
                Battlefield(P2, "Coral Barrier");

                RunGame(1);

                Equal(8, P1.Battlefield.Count);
            }
        }
    }
}
