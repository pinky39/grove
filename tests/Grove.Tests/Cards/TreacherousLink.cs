namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TreacherousLink
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantWallWithLinkToWin()
      {
        var wall = C("Wall of Blossoms");
        var link = C("Treacherous Link");

        Hand(P1, link);
        Battlefield(P1, "Swamp", "Swamp", "Grizzly Bears");        
        Battlefield(P2, wall);

        P2.Life = 2;

        RunGame(1);

        True(C(link).AttachedTo == C(wall));
        Equal(0, P2.Life);
      }
    }
  }
}