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

        Battlefield(P2, fiend.IsEquipedWith(equip));        

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, target: fiend)
            .Verify(() => Equal(0, P2.Battlefield.Count))
          );
      }

      [Fact]
      public void DoNotDestroyEnchantments()
      {
        var engine = C("Wurmcoil Engine");
        var rancor = C("Rancor");
        var bolt = C("Blastfire Bolt");

        Hand(P1, bolt);
        Battlefield(P2, engine.IsEnchantedWith(rancor));        

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, target: engine)
            .Verify(() => Equal(2, P2.Battlefield.Count))
          );
      }
    }
  }
}