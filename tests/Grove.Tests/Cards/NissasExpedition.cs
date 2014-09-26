namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class NissasExpedition
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastNissasExpedition()
            {
                Library(P1, "Island", "Mountain", "Island");
                Battlefield(P1, "Grizzly Bears", "Plains", "Plains", "Plains", "Plains");
                Hand(P1, "Nissa's Expedition", "Lava Axe");

                P2.Life = 5;
                Battlefield(P2, "Grizzly Bears");                

                RunGame(1);

                Equal(7, P1.Battlefield.Count);
            }
        }
    }
}
