namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SultaiScavenger
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastWithDelve()
      {
        var bird = C("Sultai Scavenger");
        Hand(P1, bird);
        Battlefield(P1, "Swamp", "Mountain", "Mountain", "Mountain");
        Graveyard(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        Equal(Zone.Battlefield, C(bird).Zone);
        Equal(2, P1.Exile.Count());
      }
    } 
  }
}
