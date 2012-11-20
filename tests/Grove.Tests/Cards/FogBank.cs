namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class FogBank
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void BlockDragon()
      {
        var dragon = C("Shivan Dragon");
        var bank = C("Fog Bank");
        
        Battlefield(P1, dragon);
        Battlefield(P2, bank);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dragon),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Battlefield, C(bank).Zone);
              })
        );
      }  
    }
  }
}