namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DragonStyleTwins
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Prowess()
      {
        Hand(P1, "Shock");
        Battlefield(P1, "Dragon-Style Twins", "Mountain");
        Battlefield(P2, "Grizzly Bears");
        P2.Life = 8;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}