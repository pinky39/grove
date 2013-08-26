namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;
    
  public class ThranWeaponry
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PumpAll()
      {
        Battlefield(P1, "Fleeting Image", "Fleeting Image", "Thran Weaponry", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Llanowar Elves");

        RunGame(3);

        Equal(4, P2.Life);
        Equal(6, P1.Battlefield.Lands.Count(x => !x.IsTapped));
      }
    }
  }
}