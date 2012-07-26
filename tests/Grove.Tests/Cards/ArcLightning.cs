namespace Grove.Tests.Cards
{
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class ArcLightning
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Kill2CreaturesDamagePlayer()
      {
        var elf = C("Llanowar Elves");
        var bird = C("Birds of Paradise");        
        
        var arc = C("Arc Lightning");

        Hand(P1, arc);
        Battlefield(P1, "Mountain", "Mountain", "Mountain");
        Battlefield(P2, elf, bird);   
        
        RunGame(1);     

        Equal(Zone.Graveyard, C(elf).Zone);
        Equal(Zone.Graveyard, C(bird).Zone);        
        Equal(19, P2.Life);
      }

      [Fact]
      public void KillPlayer()
      {
        var elf = C("Llanowar Elves");
        var bird = C("Birds of Paradise");        
        
        var arc = C("Arc Lightning");

        Hand(P1, arc);
        Battlefield(P1, "Mountain", "Mountain", "Mountain");
        Battlefield(P2, elf, bird);

        P2.Life = 3;
        RunGame(1);     
        
        Equal(0, P2.Life);
      }
    }
  }
}