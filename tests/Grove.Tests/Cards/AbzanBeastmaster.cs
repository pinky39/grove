namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class AbzanBeastmaster
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawTwoCards()
      {
        Battlefield(P1, "Grizzly Bears");

        Battlefield(P2, "Abzan Beastmaster", "Shivan Dragon");        

        RunGame(2);

        Equal(2, P2.Hand.Count);
      }

      [Fact]
      public void DrawOneCard()
      {
        Battlefield(P1, "Grizzly Bears", "Shivan Dragon");

        Battlefield(P2, "Abzan Beastmaster", "Grizzly Bears");

        RunGame(2);

        Equal(1, P2.Hand.Count);
      }
    }
  }
}
