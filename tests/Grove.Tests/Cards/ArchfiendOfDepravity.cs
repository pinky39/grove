namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ArchfiendOfDepravity
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayerSelectsTwoCreaturesAndSacrificeRest()
      {
        Battlefield(P1, "Archfiend of Depravity", "Wall of Frost", "Grizzly Bears", "Forest", "Forest", "Forest");

        Battlefield(P2, "Grizzly Bears", "Wall of Frost", "Grizzly Bears");

        RunGame(2);

        Equal(0, P1.Graveyard.Creatures.Count());
        Equal(1, P2.Graveyard.Creatures.Count());
      }
    }
  }
}
