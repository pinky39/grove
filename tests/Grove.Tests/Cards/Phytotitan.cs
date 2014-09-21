namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Phytotitan
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void DestroyPhytotitan()
            {
                P1.Life = 7;
                Battlefield(P1, "Grizzly Bears", "Mountain", "Mountain", "Mountain", "Mountain");

                P2.Life = 2;
                Battlefield(P2, "Phytotitan");

                RunGame(2);

                Equal(0, P2.Graveyard.Count);
                Equal(1, P2.Battlefield.Count);
                Equal(7, P1.Life);
            }
        }
    }
}
