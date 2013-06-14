namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Whetstone
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void WinTheGameByMillingOpponent()
      {
        ExileLibrary(P2);
        Library(P2, "Swamp", "Swamp");

        Battlefield(P1, "Whetstone", "Swamp", "Swamp" ,"Swamp");

        RunGame(2);

        True(P2.HasLost);
      }
    }
  }
}