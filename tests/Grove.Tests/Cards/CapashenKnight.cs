namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CapashenKnight
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PumpToKillBear()
      {
        Battlefield(P1, "Capashen Knight", "Plains", "Plains");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 2;

        RunGame(1);

        Equal(0, P1.Graveyard.Count);
        Equal(1, P2.Graveyard.Count);        
      }
    }
  }
}