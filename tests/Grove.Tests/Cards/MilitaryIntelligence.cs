namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MilitaryIntelligence
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawCardWhenAttack()
      {
        Battlefield(P1, "Military Intelligence", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
        Library(P1, "Forest", "Forest", "Forest");

        Battlefield(P2, "Military Intelligence");
        Library(P2, "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(1, P1.Hand.Count);
        Equal(0, P2.Hand.Count);
      }
    }
  }
}
