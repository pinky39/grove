namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class GoblinOffensive
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Create4Tokens()
      {
        Hand(P1, "Goblin Offensive");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");

        RunGame(1);

        Equal(4, P1.Battlefield.Count(x => x.Is("token")));
      }
    }
  }
}