namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class RoaringPrimadox
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void ReturnCreature()
            {
                Battlefield(P1, "Roaring Primadox", "Grizzly Bears");

                RunGame(1);

                Equal(1, P1.Battlefield.Count);
            }
        }
    }
}
