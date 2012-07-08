namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class SanctumGuardian
  {
    public class PredefinedAi : PredefinedAiScenario
    {
       [Fact]
      public void PreventCombatDamage()
      {
        var guardian = C("Sanctum Guardian");
         var baloth1 = C("Leatherback Baloth");
         var baloth2 = C("Leatherback Baloth");

         P2.Life = 4;

         Battlefield(P1, baloth1, baloth2);
         Battlefield(P2, guardian);

         Exec(
           At(Step.DeclareAttackers)
             .DeclareAttackers(baloth1, baloth2),
          At(Step.SecondMain)
            .Verify(()=> Equal(4, P2.Life))
         );
      }
    }
    
    public class Predefined : PredefinedScenario
    {      
      [Fact]
      public void PreventDamageFromBolt()
      {
        var guardian = C("Sanctum Guardian");
        var bolt = C("Lightning Bolt");

        Hand(P1, bolt);
        Battlefield(P2, guardian);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, P2)
            .Activate(p =>
              {
                p.Card = guardian;
                p.Targets(P2, E(bolt));
              })
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Graveyard, C(guardian).Zone);
              })
            
        );
      }
    }
  }
}