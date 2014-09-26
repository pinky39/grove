namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class LivingTotem
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastLivingTotem()
            {
                Hand(P1, "Living Totem");
                Battlefield(P1, "Forest", "Forest", "Forest", "Wall of Frost", "Grizzly Bears");

                P2.Life = 4;

                RunGame(1);

                Equal(0, P1.Hand.Count);
                Equal(1, P2.Life);
            }
        }
    }
}
