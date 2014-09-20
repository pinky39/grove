namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class WakeofDestruction
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyAllIslands()
      {
        Hand(P1, "Wake of Destruction");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Island");
        Battlefield(P2, "Mountain", "Mountain", "Island", "Island", "Island", "Island", "Island");

        RunGame(1);

        Equal(1, P1.Graveyard.Count(c => c.Name == "Island"));
        Equal(5, P2.Graveyard.Count(c => c.Name == "Island"));
      }
    }
  }
}