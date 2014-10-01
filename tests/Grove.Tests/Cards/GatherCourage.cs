namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class GatherCourage
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void CastGatherCourageForConvoke()
            {
                Hand(P1, "Gather Courage");
                Battlefield(P1, "Island", "Sunblade Elf", "Welkin Tern");

                P2.Life = 4;

                RunGame(1);

                Equal(0, P2.Life);
            }

            [Fact]
            public void CastGatherCourageForConvoke2()
            {
              Hand(P1, "Gather Courage");
              Battlefield(P1, "Island", "Wall of Frost", "Sunblade Elf", "Welkin Tern");

              P2.Life = 4;

              RunGame(1);

              Equal(0, P2.Life);
            }

            [Fact]
            public void CastGatherCourageForManaCost()
            {
                Hand(P1, "Gather Courage");
                Battlefield(P1, "Forest", "Welkin Tern");

                P2.Life = 4;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
