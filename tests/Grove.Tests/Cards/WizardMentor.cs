namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class WizardMentor
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SaveACreature()
      {
        var order = C("Order of the Sacred Bell");
        var mentor = C("Wizard Mentor");
        var bolt = C("Lightning Bolt");


        Hand(P1, bolt);
        Battlefield(P2, mentor, order);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, order)
            .Verify(()=>
              {
                Equal(Zone.Hand, C(order).Zone);
                Equal(Zone.Hand, C(mentor).Zone);
              })
          );        
      }
    }
  }
}