namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BribersPurse
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DisableOpponentCreature()
      {
        Hand(P1, "Briber's Purse");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Grizzly Bears");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}