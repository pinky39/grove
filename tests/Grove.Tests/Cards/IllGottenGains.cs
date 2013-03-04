namespace Grove.Tests.Cards
{
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class IllGottenGains
  {
    public class Ai : AiScenario
    {
      
      [Fact]
      public void Draw3CardsFromGraveyard()
      {
        var gains = C("Ill-Gotten Gains");
        Hand(P1, gains);
        Hand(P2, "Grizzly Bears", "Shock", "Shock", "Shock", "Shock");
        
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
        Graveyard(P1, "Shock", "Verdant Force", "Shock");

        RunGame(1);

        Equal(3, P1.Hand.Count);
        Equal(3, P2.Hand.Count);
        Equal(Zone.Exile, C(gains).Zone);
      }


    }
  }
}