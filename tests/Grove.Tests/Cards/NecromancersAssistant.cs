namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class NecromancersAssistant
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayNecromancerPutThreeCardsToGraveyard()
      {
        Library(P1, "Swamp", "Swamp", "Swamp");
        Hand(P1, "Necromancer's Assistant");
        Battlefield(P1, "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(3, P1.Graveyard.Count);
      }
    }
  }
}
