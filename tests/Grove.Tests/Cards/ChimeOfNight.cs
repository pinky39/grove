namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ChimeOfNight
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyBoth()
      {
        Hand(P1, "Chime of Night", "Shock");
        Battlefield(P1, "Mountain", "Swamp", "Mountain");

        Battlefield(P2, "Grizzly Bears", "Shivan Dragon");

        RunGame(1);

        Equal(2, P2.Graveyard.Count);
      }
    }
  }
}