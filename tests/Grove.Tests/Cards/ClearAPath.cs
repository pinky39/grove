namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ClearAPath
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyDefender()
      {
        Hand(P1, "Clear a Path");
        Battlefield(P1, "Mountain");
        Battlefield(P2, "Coral Barrier");

        RunGame(1);

        Equal(1, P2.Graveyard.Count);
      }
    }
  }
}