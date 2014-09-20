namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Impatience
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal2Damage()
      {
        Battlefield(P1, "Impatience");
        RunGame(2);

        Equal(18, P1.Life);
        Equal(18, P2.Life);
      }
    }
  }
}