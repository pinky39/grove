namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class AjanisPridemate
    {
        public class Predefined : PredefinedAiScenario
        {
            [Fact]
            public void GetCounter()
            {                
                var staff = C("Staff of the Sun Magus");
                var adjani1 = C("Ajani's Pridemate");
                var adjani2 = C("Ajani's Pridemate");

                Hand(P1, adjani2);
                Battlefield(P1, staff, adjani1);

                Exec(
                  At(Step.FirstMain)
                    .Cast(adjani2)
                    .Verify(() =>
                    {
                        Equal(3, C(adjani1).Power);
                        Equal(3, C(adjani1).Toughness);
                    })
                  );
            }
        }
    }
}
