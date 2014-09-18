namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Soulmender
    {
        public class Predefined : PredefinedAiScenario
        {
            [Fact]
            public void GetLife()
            {
                var soulmender = C("Soulmender");

                Battlefield(P1, soulmender);

                Exec(
                  At(Step.FirstMain)
                    .Activate(soulmender)
                    .Verify(() =>
                    {
                        Equal(true, C(soulmender).IsTapped);
                        Equal(21, P1.Life);
                    }));
            }
        }
    }
}
