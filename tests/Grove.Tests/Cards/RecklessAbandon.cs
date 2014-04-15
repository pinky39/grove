namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RecklessAbandon
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DealDamageToOpponent()
      {        
        Hand(P1, "Reckless Abandon");
        Battlefield(P1, "Llanowar Elves", "Mountain");
        P2.Life = 5;
        
        RunGame(1);

        Equal(1, P1.Graveyard.Creatures.Count());
        Equal(0, P2.Life);
      }
    }
  }
}