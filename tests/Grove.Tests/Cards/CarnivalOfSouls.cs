namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class CarnivalOfSouls
  {
    public class Ai: AiScenario
    {
      [Fact]
      public void Play2Shaddes()
      {
        Hand(P1, "Nantuko Shade", "Nantuko Shade");
        Battlefield(P1, "Carnival of Souls", "Swamp", "Swamp", "Swamp");

        RunGame(1);
        Equal(2, P1.Battlefield.Creatures.Count());
      }
    }
  }
}