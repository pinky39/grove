namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TormodsCrypt
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Scenario()
      {
        Battlefield(P1, "Tormod's Crypt");
        Graveyard(P2, "Plains", "Shield of the Avatar");

        RunGame(1);

        Equal(0, P1.Graveyard.Count);
      }
    }
  }
}
