namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SiegeDragon
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastSiegeDragon()
      {
        var wallOfFire = C("Wall of Fire");
        
        Hand(P1, "Siege Dragon");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");        
        Battlefield(P2, wallOfFire);

        RunGame(1);

        Equal(Zone.Graveyard, C(wallOfFire).Zone);        
      }

      [Fact]
      public void AttackWithSiegeDragon()
      {
        Battlefield(P1, "Siege Dragon");        
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        Equal(15, P2.Life);
        Equal(2, P2.Graveyard.Count);
      }
    }
  }
}