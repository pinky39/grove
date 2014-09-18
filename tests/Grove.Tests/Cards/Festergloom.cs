namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Festergloom
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void CastFestergloom()
            {
                var fiend = C("Torch Fiend");
                var festergloom = C("Festergloom");

                Hand(P1, festergloom);
                Battlefield(P2, fiend);                

                Exec(
                  At(Step.FirstMain)
                    .Cast(festergloom)
                    .Verify(() => { Equal(0, P2.Battlefield.Count); })
                  );
            }
        }
    }
}
