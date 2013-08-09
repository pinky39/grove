namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class BrinkOfMadness
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacrificeDiscardOpponentsHand()
      {
        var brink = C("Brink Of Madness");

        Battlefield(P1, brink);
        Hand(P2, "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(3, P2.Graveyard.Count);
        Equal(Zone.Graveyard, C(brink).Zone);
      }
    }
  }
}