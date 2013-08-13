namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class IronMaiden
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal3Damage()
      {
        Hand(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Iron Maiden");

        RunGame(1);

        Equal(17, P1.Life);
      }
    }
  }
}