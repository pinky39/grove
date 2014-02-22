namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class GoblinMatron
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void FetchAGoblin()
      {
        var buggy = C("Goblin War Buggy");
        Library(P1, buggy, "Forest", "Forest");
        Hand(P1, "Goblin Matron");
        Battlefield(P1, "Forest", "Mountain", "Mountain");

        RunGame(1);

        Equal(Zone.Hand, C(buggy).Zone);
      }
    }
  }
}