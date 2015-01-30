namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class KinTreeInvocation
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Put22Counter()
      {
        Hand(P1, "Kin-Tree Invocation");
        Battlefield(P1, "Typhoid Rats", "Grizzly Bears", "Swamp", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count(x => x.Power == 2));
      }
    }
  }
}
