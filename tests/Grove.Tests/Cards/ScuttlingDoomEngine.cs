namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ScuttlingDoomEngine
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void UnblockableForBear()
      {
        Battlefield(P1, "Scuttling Doom Engine");

        P2.Life = 6;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(0, P2.Life);        
      }

      [Fact]
      public void CanBeBlockedByDragon()
      {
        Battlefield(P1, "Scuttling Doom Engine");

        P2.Life = 6;
        Battlefield(P2, "Shivan Dragon");

        RunGame(1);

        Equal(6, P2.Life);
        Equal(0, P2.Battlefield.Count);
      }
    }
  }
}
