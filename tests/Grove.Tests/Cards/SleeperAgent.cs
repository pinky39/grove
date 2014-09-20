namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SleeperAgent
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AgentDeal2DamageToOpponent()
      {
        var agent = C("Sleeper Agent");
        Hand(P1, agent);
        Battlefield(P1, "Swamp", "Wall of Blossoms");

        RunGame(4);

        Equal(P2, C(agent).Controller);
        Equal(20, P1.Life);
        Equal(16, P2.Life);
      }
    }
  }
}