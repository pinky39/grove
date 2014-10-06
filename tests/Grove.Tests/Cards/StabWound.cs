namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class StabWound
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void PlayStabWound()
            {
                Hand(P1, "Stab Wound");
                Battlefield(P1, "Swamp", "Mountain", "Mountain");

                Battlefield(P2, "Wall of Fire");

                RunGame(2);

                Equal(0, P1.Hand.Count);
                Equal(20, P1.Life);
                Equal(18, P2.Life);
            }

        }
    }
}
