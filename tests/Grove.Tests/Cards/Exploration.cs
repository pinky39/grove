namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Exploration
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Play1Land()
      {
        Hand(P1, "Forest", "Forest", "Forest");
        RunGame(1);
        Equal(1, P1.Battlefield.Lands.Count());
      }

      [Fact]
      public void Play2Lands()
      {
        Hand(P1, "Forest", "Forest", "Forest", "Exploration");
        RunGame(1);
        Equal(2, P1.Battlefield.Lands.Count());
      }
    }
  }
}