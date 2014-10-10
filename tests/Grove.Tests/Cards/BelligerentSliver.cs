namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BelligerentSliver
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CannotBeBlockedBy1Creature()
      {
        Battlefield(P1, "Belligerent Sliver", "Leeching Sliver");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Graveyard.Count);
        Equal(0, P1.Graveyard.Count);
        Equal(0, P2.Life);
      }

      [Fact]
      public void CanBeBlockedBy2Creatures()
      {
        Battlefield(P1, "Belligerent Sliver");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 2;

        RunGame(1);

        Equal(2, P2.Life);
      }
    }
  }
}
