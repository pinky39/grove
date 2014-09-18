namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class BlastfireBolt
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void DamageAndDestroyEquipment()
            {
                var fiend = C("Torch Fiend");
                var equip = C("Basilisk Collar");
                var bolt = C("Blastfire Bolt");

                Hand(P1, bolt);

                Battlefield(P2, fiend, equip);
                EquipCard(fiend, equip);                

                Exec(
                  At(Step.FirstMain)
                    .Cast(bolt, target: fiend)
                    .Verify(() => { Equal(0, P2.Battlefield.Count); })
                  );
            }
        }
    }
}
