namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SwordOfBodyAndMind
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void MillAndTokens()
      {
        Battlefield(P1,C("Rumbling Slum").IsEquipedWith(C("Sword of Body and Mind")));
        
        RunGame(1);

        Equal(12, P2.Life);
        Equal(2, P1.Battlefield.Creatures.Count());
        Equal(50, P2.Library.Count());
        Equal(10, P2.Graveyard.Count());     
      }  
    }
  }
}