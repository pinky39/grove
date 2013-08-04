namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class FranticSearch
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastSearchCastFaeries()
      {
        var faeries = C("Weatherseed Faeries");
        
        Hand(P1, "Frantic Search", "Island", "Island", "Island", "Island");        
        Library(P1, "Island", faeries);
        
        Battlefield(P1, "Island", "Island");

        RunGame(1);

        Equal(Zone.Battlefield, C(faeries).Zone);
      }
    }
  }
}