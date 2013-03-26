namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Opression
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BothPlayersAreAffected()
      {
        var shock1 = C("Shock");
        var shock2 = C("Shock");
        
        Hand(P1, shock1, "Grizzly Bears");
        Hand(P2, shock2, "Grizzly Bears");

        Battlefield(P1, "Mountain", "Opression");
        Battlefield(P2, "Mountain");

        P1.Life = 3;
        P2.Life = 3;

        RunGame(2);

        Equal(2, P1.Graveyard.Count);
        Equal(2, P2.Graveyard.Count);
      }
    }
  }
}