namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class GraveTitan
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void MakeSomeZombies1()
      {
        Hand(P1, "Grave Titan");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

        RunGame(maxTurnCount: 3);

        Equal(5, P1.Battlefield.Creatures.Count());
        Equal(10, P2.Life);
      }
    }
  }
}