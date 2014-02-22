namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class SwordsToPlowshares
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ExileForce()
      {
        var force = C("Verdant Force");

        Battlefield(P1, "Plains");
        Hand(P1, "Swords to Plowshares");
        Battlefield(P2, force);

        RunGame(maxTurnCount: 2);

        Equal(Zone.Exile, C(force).Zone);
        Equal(27, P2.Life);
      }
    }
  }
}