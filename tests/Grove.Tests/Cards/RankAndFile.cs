namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class RankAndFile
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GreenCreaturesGetM1M1()
      {                
        Hand(P1, "Rank and File");
        Battlefield(P1, "Llanowar Elves", "Swamp", "Swamp", "Forest", "Forest");
        Battlefield(P2, "Llanowar Elves", "Birds of Paradise");

        RunGame(1);

        Equal(1, P1.Graveyard.Count);
        Equal(2, P2.Graveyard.Count);
      }
    }
  }
}