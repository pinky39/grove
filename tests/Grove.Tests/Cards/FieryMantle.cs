namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class FieryMantle
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BearDeals6Damage()
      {
        Hand(P1, "Fiery Mantle");
        Battlefield(P1, "Grizzly Bears", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");

        RunGame(1);

        Equal(14, P2.Life);
      }
    }
  }
}