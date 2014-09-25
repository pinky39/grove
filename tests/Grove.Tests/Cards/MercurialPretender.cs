namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class MercurialPretender
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void PlayAsCopy()
            {
                var card = C("Mercurial Pretender");
                Hand(P1, card);
                Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Resolute Archangel");

                Battlefield(P2, "Resolute Archangel");

                RunGame(1);

                Equal(7, P1.Battlefield.Count);
                Equal(0, P1.Graveyard.Count);
                False(P1.Battlefield.Creatures.Any(c => c.Colors.Contains(CardColor.Blue)));
                True(P1.Battlefield.Creatures.Any(c => c.Colors.Contains(CardColor.White)));
                True(P1.Battlefield.Creatures.Any(c => c.Has().Flying));
            }
        }
    }
}
