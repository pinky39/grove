namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Eradicate
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Exile()
      {
        Hand(P1, "Eradicate");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Llanowar Behemoth");
        Graveyard(P2, "Llanowar Behemoth");
        Library(P2, "Llanowar Behemoth");
        Hand(P2, "Llanowar Behemoth");

        RunGame(1);

        Equal(4, P2.Exile.Count(x => x.Name.Equals("Llanowar Behemoth")));
      }
    }
  }
}