namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class BloodHost
    {
        public class Predefined : PredefinedAiScenario
        {
            [Fact]
            public void Sacrifice()
            {
                var bloodHost = C("Blood Host");
                var wall = C("Grizzly Bears");

                Battlefield(P1, bloodHost, wall);

                Exec(
                  At(Step.FirstMain)
                    .Activate(bloodHost, costTarget: wall)
                    .Verify(() =>
                    {
                        Equal(1, P1.Battlefield.Creatures.Count());
                        Equal(22, P1.Life);
                        Equal(4, C(bloodHost).Power);
                        Equal(4, C(bloodHost).Toughness);                                                
                    }));               
            }
        }
    }
}
