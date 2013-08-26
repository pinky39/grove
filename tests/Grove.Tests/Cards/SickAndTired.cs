namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SickAndTired
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Kill2Elves()
      {
        Hand(P1, "Sick and Tired");
        Battlefield(P1, "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Llanowar Elves", "Llanowar Elves");
        
        RunGame(2);

        Equal(2, P2.Graveyard.Count);
      }
    }
  }
}