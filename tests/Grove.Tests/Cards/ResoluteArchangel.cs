namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class ResoluteArchangel
    {
        public class Predefined : PredefinedAiScenario
        {
            [Fact]
            public void SetLife()
            {
                var angel = C("Resolute Archangel");

                P1.Life = 10;
                Hand(P1, angel);

                Exec(
                  At(Step.FirstMain)
                    .Cast(angel)
                    .Verify(() =>
                    {
                        Equal(1, P1.Battlefield.Creatures.Count());
                        Equal(20, P1.Life);
                    }));              
            }
        }
    }
}
