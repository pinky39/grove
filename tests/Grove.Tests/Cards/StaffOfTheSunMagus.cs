namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class StaffOfTheSunMagus
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void GetLives()
            {
                Hand(P1, "Plains", "Kinsbaile Skirmisher");
                Battlefield(P1, "Staff of the Sun Magus", "Plains");

                Battlefield(P2, "Grizzly Bears");

                RunGame(1);

                Equal(22, P1.Life);
            }
        }
    }
}
