namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Repopulate
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ShuffleCreaturesToYourLibraryToBringThemBack()
      {
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
        
        Hand(P2, "Repopulate");
        Graveyard(P2, "Shivan Hellkite", "Shivan Hellkite");
        Battlefield(P2, "Defense of the Heart", "Forest", "Forest");

        RunGame(2);

        Equal(2, P2.Battlefield.Creatures.Count());                        
      }
    }
  }
}