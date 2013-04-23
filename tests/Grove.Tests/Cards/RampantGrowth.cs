namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RampantGrowth
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutMountainIntoPlayTapped()
      {
        Library(P1, "Mountain", "Mountain");
        Hand(P1, "Rampant Growth");
        Battlefield(P1, "Forest", "Mountain");

        RunGame(1);

        Equal(2, P1.Battlefield.Count(x => x.Is("mountain") && x.IsTapped));        
      }
    }
  }
}