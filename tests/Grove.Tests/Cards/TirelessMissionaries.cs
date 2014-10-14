namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TirelessMissionaries
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Gain3Life()
      {
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains");
        Hand(P1, "Tireless Missionaries");

        RunGame(1);

        Equal(23, P1.Life);
      }
    }
  }
}