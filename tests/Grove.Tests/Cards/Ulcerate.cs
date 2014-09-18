namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Ulcerate
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void Cast()
            {
                var ulcerate = C("Ulcerate");
                var bear = C("Grizzly Bears");

                Hand(P1, ulcerate);
                Battlefield(P2, bear);

                Exec(
                  At(Step.FirstMain)
                    .Cast(ulcerate, target: bear)
                    .Verify(() =>
                    {
                        Equal(0, P2.Battlefield.Count());
                        Equal(17, P1.Life);
                    })
                  );
            }
        }
    }
}
