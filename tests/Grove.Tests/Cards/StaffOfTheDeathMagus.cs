namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class StaffOfTheDeathMagus
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void GetLives()
            {
                Hand(P1, "Swamp", "Child of Night");
                Battlefield(P1, "Staff of the Death Magus", "Swamp");

                Battlefield(P2, "Grizzly Bears");

                RunGame(1);

                Equal(22, P1.Life);
            }
        }
    }
}
