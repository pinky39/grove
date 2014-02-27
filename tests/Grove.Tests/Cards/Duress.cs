namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Duress
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DiscardOneCard()
      {
        var counterspell = C("Counterspell");

        Hand(P1, "Duress");
        Battlefield(P1, "Swamp");

        Hand(P2, counterspell, "Island");

        RunGame(1);

        Equal(Zone.Graveyard, C(counterspell).Zone);
      }
    }
  }
}