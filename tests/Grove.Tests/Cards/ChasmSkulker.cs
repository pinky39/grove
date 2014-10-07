namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ChasmSkulker
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawCardAddCounter()
      {
        var skulker = C("Chasm Skulker");

        Library(P1, "Swamp");

        Library(P2, "Island");
        Battlefield(P2, skulker);

        RunGame(3);

        Equal(2, C(skulker).Power);
      }

      [Fact]
      public void DestroySkulkerCreateTokens()
      {
        var skulker = C("Chasm Skulker");

        P1.Life = 2;
        Battlefield(P1, "Grizzly Bears");

        Library(P2, "Island");
        Battlefield(P2, skulker);

        RunGame(3);

        Equal(0, P1.Battlefield.Count);
        True(C(skulker).Zone == Zone.Graveyard);
        Equal(1, P2.Battlefield.Creatures.Count(c => c.Is().Token));
      }
    }
  }
}
