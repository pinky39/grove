namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class UrzasIncubator
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Cast2Baloths()
      {
        Hand(P1, "Urza's Incubator", "Ravenous Baloth", "Ravenous Baloth");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count());
      }
    }
  }
}