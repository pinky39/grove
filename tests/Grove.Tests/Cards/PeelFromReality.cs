namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PeelFromReality
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnBearAndDragon()
      {
        var dragon = C("Shivan Dragon");
        var bears = C("Grizzly Bears");

        Battlefield(P1, dragon);
        Hand(P2, "Peel from Reality");
        Battlefield(P2, "Island", "Island", "Island", "Island", bears, "Verdant Force");

        P2.Life = 5;

        RunGame(1);

        Equal(Zone.Hand, C(bears).Zone);
        Equal(Zone.Hand, C(dragon).Zone);
      }
    }
  }
}