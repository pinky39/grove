namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Persecute
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DiscardAllBlue()
      {
        Hand(P1, "Persecute");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
        Hand(P2, "Counterspell", "Counterspell", "Mana Leak");
        Battlefield(P2, "Island", "Fog Bank");

        RunGame(1);

        Equal(3, P2.Graveyard.Count);
      }
    }
  }
}