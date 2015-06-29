namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TormodsCrypt
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PreventGravediggerFromBringingCreaturesBack()
      {
        Battlefield(P1, "Tormod's Crypt");
        
        Hand(P2, "Gravedigger");
        Battlefield(P2, "Swamp", "Plains", "Plains", "Plains", "Plains");
        Graveyard(P2, "Serra Angel", "Shivan Dragon", "Plains", "Plains");

        RunGame(2);

        Equal(0, P2.Graveyard.Count);
      }
    }
  }
}
