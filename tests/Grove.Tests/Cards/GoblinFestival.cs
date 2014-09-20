namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GoblinFestival
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillOpponent()
      {
        Battlefield(P1, "Goblin Festival", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        P2.Life = 3;
        
        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}