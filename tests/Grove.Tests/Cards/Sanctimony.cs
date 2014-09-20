namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Sanctimony
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Gain4Life()
      {
        Hand(P1, "Shivan Dragon");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Forest", "Forest");
        Battlefield(P2, "Sanctimony");

        RunGame(3);

        Equal(19, P2.Life);
      }
    }
  }
}