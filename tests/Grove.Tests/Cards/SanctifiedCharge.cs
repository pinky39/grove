namespace Grove.Tests.Cards
{
  using System.Collections.Generic;
  using Infrastructure;
  using Xunit;

  public class SanctifiedCharge
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastChargeToKill()
      {
        var lions = C("Savannah Lions");
        
        Hand(P1, "Sanctified Charge");        
        Battlefield(P1, "Grizzly Bears", lions, "Plains", "Mountain", "Mountain", 
          "Mountain", "Mountain", "Forest", "Mountain", "Mountain", "Mountain", "Mountain");

        Battlefield(P2, "Skittering Skirge");

        P1.Life = 3;
        P2.Life = 8;
        
        RunGame(2);

        Equal(0, P2.Life);
        True(C(lions).Has().FirstStrike);
      }
    }
  }
}