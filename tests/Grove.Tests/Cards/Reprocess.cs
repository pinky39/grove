namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Reprocess
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawCards()
      {
        Hand(P1, "Reprocess");
        Battlefield(P1, "Forest", "Forest", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Llanowar Elves", "Grizzly Bears", "Fecundity");

        RunGame(1);

        Equal(7, P1.Hand.Count);
      }
    }
  }
}