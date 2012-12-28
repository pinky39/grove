namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GoblinLackey
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutGoblinToPlay()
      {
        Hand(P1, "Goblin War Buggy");
        Battlefield(P1, "Goblin Lackey");

        RunGame(1);

        Equal(2, P1.Battlefield.Count);
      }
    }
  }
}