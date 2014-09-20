namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TragicPoet
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BringBackEmbrace()
      {
        Battlefield(P1, "Tragic Poet", "Grizzly Bears", "Mountain", "Mountain", "Mountain", "Mountain");
        Graveyard(P1, "Shiv's Embrace");
        Battlefield(P2, "Wall of Blossoms");
        P2.Life = 4;
        
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}