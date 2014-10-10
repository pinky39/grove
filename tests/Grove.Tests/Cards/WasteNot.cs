namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class WasteNot
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DiscardBearsPutTokens()
      {
        Hand(P1, "Mind Rot");
        Battlefield(P1, "Waste Not", "Swamp", "Swamp", "Swamp");

        Hand(P2, "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count(c => c.Is().Token));
      }

      [Fact]
      public void DiscardLandGetMana()
      {
        Hand(P1, "Mind Rot", "Juggernaut");
        Battlefield(P1, "Waste Not", "Swamp", "Swamp", "Swamp");

        Hand(P2, "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(1, P1.Battlefield.Creatures.Count());
        Equal(0, P1.Battlefield.Creatures.Count(c => c.Is().Token));
      }

      [Fact]
      public void DiscardArtifactDrawCard()
      {
        Library(P1, "Juggernaut", "Juggernaut");
        Hand(P1, "Mind Rot", "Juggernaut");
        Battlefield(P1, "Waste Not", "Swamp", "Swamp", "Swamp");

        Hand(P2, "Profane Memento", "Profane Memento");

        RunGame(1);

        Equal(0, P1.Battlefield.Creatures.Count());
        Equal(0, P1.Battlefield.Creatures.Count(c => c.Is().Token));
        Equal(3, P1.Hand.Count(c => c.Name == "Juggernaut"));
      }
    }
  }
}
