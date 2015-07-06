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

      [Fact]
      public void EquipementShouldNotChangeBattlefieldsBug()
      {
        Hand(P1, "Endless Obedience");
        Battlefield(P1, "Brawler's Plate", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

        P2.Life = 7;
        Graveyard(P2, "Shivan Dragon");
        Battlefield(P2, "Plains", "Plains", "Plains", "Plains");
        Hand(P2, "Divine Verdict");

        RunGame(3);
      }
    }
  }
}
