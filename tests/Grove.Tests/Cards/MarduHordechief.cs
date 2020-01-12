namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class MarduHordechief
  {
    public class Ai: AiScenario
    {
      [Fact]
      public void Create11Token()
      {
        Hand(P1, "Mardu Hordechief");
        Battlefield(P1, "Plains", "Forest", "Plains", "Grizzly Bears");

        RunGame(1);

        Assert.Equal(3, P1.Battlefield.Creatures.Count());
      }
    }
  }
}
