namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class NissasExpedition
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Put2LandsInPlay()
      {
        Library(P1, "Grizzly Bears", "Island", "Mountain", "Island");
        Battlefield(P1, "Grizzly Bears", "Plains", "Plains", "Plains", "Plains");
        Hand(P1, "Nissa's Expedition");
        
        RunGame(1);

        Equal(6, P1.Battlefield.Lands.Count());
      }
    }
  }
}