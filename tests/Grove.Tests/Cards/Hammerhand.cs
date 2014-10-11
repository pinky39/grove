namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Hammerhand
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantBearTapTarget()
      {
        Hand(P1, "Hammerhand");
        Battlefield(P1, "Grizzly Bears", "Mountain");

        P2.Life = 3;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
