namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Negate
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CounterMeteorite()
      {
        var meteorite = C("Meteorite");
        
        Hand(P1, meteorite);
        Hand(P2, "Negate");
        
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island");
        Battlefield(P2, "Island", "Island");

        P1.Life = 2;
        
        RunGame(1);
        Equal(2, P1.Life);
        Equal(Zone.Graveyard, C(meteorite).Zone);
      }
    }
  }
}