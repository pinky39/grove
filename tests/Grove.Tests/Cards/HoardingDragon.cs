namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class HoardingDragon
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutEnginetoToHand()
      {
        // Turn 1: Cast Hoarding Dragon. Wurmcoil Engine is exiled
        // Turn 2: Draw card
        // Turn 3: Draw card. Attack with dragon. Opponent casts Flesh to Dust. Artifact is put into Hand
        
        var engine = C("Wurmcoil Engine");
        
        Hand(P1, "Hoarding Dragon");
        Library(P1, "Grizzly Bears", "Grizzly Bears", engine);
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");

        P2.Life = 4;
        Hand(P2, "Flesh to Dust");
        Battlefield(P2, "Swamp", "Swamp", "Mountain", "Mountain", "Mountain");

        RunGame(3);        
        Equal(Zone.Hand, C(engine).Zone);                
      }
    }
  }
}