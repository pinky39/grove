namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class MeditationPuzzle
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastMeditationPuzzle()
            {
                Battlefield(P1, "Grizzly Bears", "Grizzly Bears");

                P2.Life = 2;
                Battlefield(P2, "Grizzly Bears", "Plains", "Plains", "Plains", "Plains");
                Hand(P2, "Meditation Puzzle");

                RunGame(1);

                Equal(6, P2.Life);
            }
        }
    }
}
