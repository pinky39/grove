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
        var force = C("Verdant Force");

        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Hand(P1, "Endless Obedience");
        
        Graveyard(P2, force);

        RunGame(1);

        Equal(Zone.Battlefield, C(force).Zone);
        Equal(P1, C(force).Controller);
      }
    }
  }
}
