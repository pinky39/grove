namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class EphemeralShields
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void IndestructibleShield()
            {
                Battlefield(P1, "Grizzly Bears", "Wall of Frost", "Plains");
                Hand(P1, "Ephemeral Shields");

                P2.Life = 2;
                Battlefield(P2, "Swamp", "Swamp");
                Hand(P2, "Doom blade");

                RunGame(1);

                Equal(0, P2.Hand.Count);
                Equal(0, P2.Life);
                Equal(3, P1.Battlefield.Count);
            }
        }
    }
}
