namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BrawlersPlate
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EquipWithBrawlersPlate()
      {
        Battlefield(P1, "Grizzly Bears", "Brawler's Plate", "Mountain", "Mountain", "Mountain", "Mountain");

        P2.Life = 4;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(2, P2.Life);
        Equal(1, P2.Graveyard.Count);
      }
    }
  }
}