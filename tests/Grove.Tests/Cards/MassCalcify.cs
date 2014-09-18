namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class MassCalcify
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void DestroyAll()
            {
                Hand(P1, "Mass Calcify");
                Battlefield(P1, "Kinsbaile Skirmisher", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");

                P2.Life = 2;
                Battlefield(P2, "Grizzly Bears");

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
