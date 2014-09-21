namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Encrust
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void Encrust()
            {
                var encrust = C("Encrust");
                var bear = C("Grizzly Bears");

                Hand(P1, encrust);
                Battlefield(P1, "Island", "Island", "Island");

                Battlefield(P2, bear);

                RunGame(1);
                
                Equal(0, P1.Hand.Count);
            }
        }
    }
}
