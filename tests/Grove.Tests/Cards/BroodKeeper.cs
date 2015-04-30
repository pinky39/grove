namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class BroodKeeper
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void RancorTheBroodKeeper()
      {
        Hand(P1, "Rancor");
        Battlefield(P1, "Brood Keeper", "Forest");

        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count());
      }
    }
  }
}