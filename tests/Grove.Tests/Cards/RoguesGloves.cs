namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class RoguesGloves
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttachAndDrawACard()
      {
        Battlefield(P1, "Rogue's Gloves", "Grizzly Bears", "Forest", "Forest");

        RunGame(1);
        Equal(1, P1.Hand.Count);
      }
    }
  }
}