namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ArrowStorm
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ArrowStormDeals5DamageAfterAttack()
      {
        Battlefield(P1, "Grizzly Bears", "Mountain", "Forest", "Mountain", "Forest", "Mountain", "Forest");
        Hand(P1, "Arrow Storm");

        P2.Life = 7;
      
        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void ArrowStormCannotBePrevented()
      {
        Battlefield(P1, "Grizzly Bears", "Mountain", "Forest", "Mountain", "Forest", "Mountain", "Forest");
        Hand(P1, "Arrow Storm");

        P2.Life = 6;
        Battlefield(P2, "Urza's Armor");

        // Bears deal 1 damage (1 prevented) and Storm deals 5 damage (not prevented)
        RunGame(1);
        
        Equal(0, P2.Life);
      }
    }
  }
}
