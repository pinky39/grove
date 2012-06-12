namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class MirranCrusader
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DoubleStrike1()
      {
        Battlefield(P1, "Mirran Crusader", "Mirran Crusader", "Llanowar Elves");
        Battlefield(P2, "Savannah Lions", "Savannah Lions");

        P2.Life = 4;

        RunGame(maxTurnCount: 1);

        Equal(0, P2.Battlefield.Count());
        Equal(3, P1.Battlefield.Count());
        Equal(3, P2.Life);        
        
      }

      [Fact]
      public void DoubleStrike2()
      {
        Battlefield(P1, "Mirran Crusader", "Llanowar Elves");
        Battlefield(P2, "Savannah Lions", "Savannah Lions");

        P2.Life = 20;

        RunGame(maxTurnCount: 1);

        Equal(0, P2.Battlefield.Count());
        Equal(1, P1.Battlefield.Count());
        Equal(20, P2.Life);        
      }
    }
  }
}