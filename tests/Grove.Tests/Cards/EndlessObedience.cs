namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class EndlessObedience
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnForceFromOpponentsGraveyard()
      {
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Hand(P1, "Endless Obedience");
        
        Graveyard(P2, "Verdant Force");

        RunGame(1);

        Equal(1, P1.Battlefield.Creatures.Count());
      }
    }
  }
}
