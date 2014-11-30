namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AvacynGuardianAngel
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PreventDamageFromRedSourceToPegasus()
      {
        var bolt = C("Lightning Bolt");
        var pegasus = C("Pegasus Charger");
        
        Hand(P1, bolt);
        Battlefield(P2, pegasus, "Avacyn, Guardian Angel", "Plains", "Plains");

        Exec(
        At(Step.FirstMain)
          .Cast(bolt, target: pegasus),
        At(Step.SecondMain)
          .Verify(() => Equal(Zone.Battlefield, C(pegasus).Zone))
        );
      }

      [Fact]
      public void PreventDamageFromRedSourceToPlayer()
      {
        var bolt = C("Lightning Bolt");        

        Hand(P1, bolt);
        Battlefield(P2, "Avacyn, Guardian Angel", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        
        Exec(
        At(Step.FirstMain)
          .Cast(bolt, target: P2),
        At(Step.SecondMain)
          .Verify(() => Equal(20, P2.Life))
        );
      }

      [Fact]
      public void PreventDamageFromDragonToPegasus()
      {        
        var pegasus = C("Pegasus Charger");
        var dragon = C("Shivan Dragon");

        Battlefield(P1, dragon.IsEnchantedWith("Blanchwood Armor"), "Grizzly Bears", "Forest", "Forest");
        Battlefield(P2, pegasus, "Avacyn, Guardian Angel", "Plains", "Plains");

        Exec(
        At(Step.DeclareAttackers)
          .DeclareAttackers(dragon),
        At(Step.SecondMain)
          .Verify(() =>
            {
              Equal(20, P2.Life);
              Equal(Zone.Battlefield, C(pegasus).Zone);
            })
        );
      }
    }
  }
}