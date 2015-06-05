namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class KalonianTwingrove
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GroveAndTwinAre66()
      {
        Hand(P1, "Kalonian Twingrove");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count(x => x.Power == 6 && x.Toughness == 6));
      }
    }
  }
}