namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SpiritBonds
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayCreatureGetToken()
      {
        Hand(P1, "Grizzly Bears");
        Battlefield(P1, "Spirit Bonds", "Plains", "Plains", "Forest");

        RunGame(1);

        Equal(1, P1.Battlefield.Creatures.Count(c => c.Is().Token));
      }
    }
  }
}
