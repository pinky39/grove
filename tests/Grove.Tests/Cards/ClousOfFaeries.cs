namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ClousOfFaeries
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Untap2Lands()
      {
        Hand(P1, "Cloud of Faeries");
        Battlefield(P1, "Remote Isle", "Forest", "Forest");

        RunGame(1);

        Equal(1, P1.Battlefield.Creatures.Count());
        Equal(0, P1.Battlefield.Lands.Count(x => x.IsTapped));
      }
    }
  }
}