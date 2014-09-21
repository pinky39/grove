namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class WelkinTern
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void BlockWithWelkinTern()
            {
                Battlefield(P1, "Juggernaut");

                P2.Life = 5;
                Battlefield(P2, "Welkin Tern");

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
