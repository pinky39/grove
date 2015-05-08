namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ParagonOfOpenGraves
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveDebaserDeathtouchToKillDragon()
      {
        var dragon = C("Shivan Dragon");

        Battlefield(P1, "Paragon of Open Graves", "Phyrexian Debaser", "Swamp", "Swamp", "Swamp");        
        Battlefield(P2, dragon);
        
        P2.Life = 3;

        RunGame(3);

        Equal(Zone.Graveyard, C(dragon).Zone);        
      }

      [Fact]
      public void CannotActivateStaticAbilityOfParagonTwice()
      {
        var paragon = C("Paragon of Open Graves");
        var rat = C("Typhoid Rats");
        var bolt = C("Lightning Bolt");

        Hand(P1, "Endless Obedience");
        Battlefield(P1, paragon, rat, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

        P2.Life = 2;
        Hand(P2, bolt);
        Battlefield(P2, "Mountain");

        RunGame(1);

        Equal(1, P2.Life);
        Equal(Zone.Battlefield, C(paragon).Zone);
        Equal(Zone.Graveyard, C(bolt).Zone);
        Equal(2, C(rat).Power);
      }
    }
  }
}