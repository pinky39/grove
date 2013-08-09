namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BouncingBeebles
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Unblockable()
      {
        Battlefield(P1, "Bouncing Beebles");
        Battlefield(P2, "Dragon Blood", "Grizzly Bears");

        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}