namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ShamanOfSpring
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void DrawCard()
            {
                Hand(P1, "Shaman Of Spring");
                Library(P1, "Grizzly Bears");
                Battlefield(P1, "Forest", "Forest","Forest","Forest");

                RunGame(1);

                Equal(5, P1.Battlefield.Count);
                Equal(1, P1.Hand.Count);
            }
        }
    }
}
