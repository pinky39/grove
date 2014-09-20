namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class VeiledCrocodile
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayBearAttackWithCrocodile()
      {
        Hand(P1, "Grizzly Bears");
        Battlefield(P1, "Veiled Crocodile", "Forest", "Forest");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void CrocodileLastCardInHand()
      {
        var crocodile = C("Veiled Crocodile");
        Hand(P1, crocodile);
        Battlefield(P1, "Island", "Island", "Island");

        RunGame(1);

        Equal(4, C(crocodile).Power);
      }
    }
        
  }
}