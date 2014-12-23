namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class HushwingGryff
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastGryffSoOpponentDoesNotGetSquid()
      {
        var barrier = C("Coral Barrier");
        var gryff = C("Hushwing Gryff");
        
        Hand(P1, barrier);
        Battlefield(P1, "Island", "Island", "Island");
        
        Hand(P2, gryff);
        Battlefield(P2, "Island", "Island", "Plains");

        RunGame(1);

        Equal(Zone.Battlefield, C(barrier).Zone);
        Equal(Zone.Battlefield, C(gryff).Zone);
        Equal(1, P1.Battlefield.Creatures.Count());
      }
    }
  }
}