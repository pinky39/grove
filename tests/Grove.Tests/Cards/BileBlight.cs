namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BileBlight
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyAllGrizzlies()
      {
        Hand(P1, "Bile Blight");
        Battlefield(P1, "Runeclaw Bear", "Swamp", "Swamp");

        P2.Life = 2;
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        Equal(0, P2.Life);
        Equal(3, P2.Graveyard.Count);
      }
    }
  }
}
