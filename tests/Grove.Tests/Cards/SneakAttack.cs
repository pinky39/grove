namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SneakAttack
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackHero()
      {
        var hero = C("Hero of Bladehold");
        
        Battlefield(P1, "Sneak Attack", "Mountain");        
        Hand(P1, hero);
        P2.Life = 8;

        RunGame(1);

        Equal(1, P2.Life);
        Equal(Zone.Graveyard, C(hero).Zone);
      }
    }
  }
}