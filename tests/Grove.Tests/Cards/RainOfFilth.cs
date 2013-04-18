namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class RainOfFilth
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastTitan()
      {
        Hand(P1, "Grave Titan", "Rain of Filth");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");

        RunGame(3);
        Equal(10, P2.Life);
      }
    }
  }
}