namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Rewind
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CounterForce()
      {
        Hand(P2, "Rewind");
        Hand(P1, "Verdant Force");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Island", "Island", "Island", "Island");

        RunGame(1);

        Equal(4, P2.Battlefield.Count(x => x.Is().Land && !x.IsTapped));
        Equal(1, P1.Graveyard.Count);
        Equal(1, P2.Graveyard.Count);
      }

      [Fact]
      public void DoNotUntapLandsIfCountered()
      {
        Hand(P2, "Rewind");
        Hand(P1, "Verdant Force", "Mana Leak");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Island",
          "Island");
        Battlefield(P2, "Island", "Island", "Island", "Island");

        RunGame(1);

        Equal(4, P2.Battlefield.Count(x => x.Is().Land && x.IsTapped));
        Equal(1, P1.Graveyard.Count);
        Equal(1, P2.Graveyard.Count);
      }
    }
  }
}