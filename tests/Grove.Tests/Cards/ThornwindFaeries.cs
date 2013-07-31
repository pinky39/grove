namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class ThornwindFaeries
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillRaptor()
      {
        var raptor = C("Shivan Raptor");
        
        Battlefield(P1, "Thornwind Faeries", "Grizzly Bears");        
        Battlefield(P2, raptor);

        RunGame(1);

        Equal(Zone.Graveyard, C(raptor).Zone);
      }
    }
  }
}