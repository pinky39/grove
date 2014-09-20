namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SoulFeast
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TargetPlayerLooses4LifeYouGain4Life()
      {
        Hand(P1, "Soul Feast");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        P2.Life = 4;
        
        RunGame(1);

        Equal(0, P2.Life);
        Equal(24, P1.Life);
      }
    }
  }
}