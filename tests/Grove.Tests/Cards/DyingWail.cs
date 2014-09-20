namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DyingWail
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantRats()
      {
        Hand(P1, "Dying Wail");
        Battlefield(P1, "Ravenous Rats", "Swamp", "Swamp");
        Battlefield(P2, "Llanowar Elves");
        Hand(P2, "Swamp", "Swamp");
        P2.Life = 1;

        RunGame(1);
        Equal(0, P2.Hand.Count);
        Equal(2, P1.Graveyard.Count);
      }
    }
  }
}