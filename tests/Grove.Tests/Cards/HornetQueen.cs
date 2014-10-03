namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class HornetQueen
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Put4TokensIntoPlay()
      {
        Hand(P1, "Hornet Queen");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(5, P1.Battlefield.Creatures.Count());
      }
    }
  }
}