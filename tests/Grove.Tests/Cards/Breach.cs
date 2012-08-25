namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Breach
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DealKillingBlow()
      {
        Hand(P1, "Breach");
        Battlefield(P1, "Grizzly Bears", "Llanowar Elves", "Llanowar Elves", "Swamp", "Forest", "Forest");
        Battlefield(P2, "Grizzly Bears", "Shivan Dragon");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}