namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HuntingMoa
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Put11CounterOfFaeries()
      {
        Hand(P1, "Hunting Moa");
        Battlefield(P1, "Cloud of Faeries", "Forest", "Forest", "Forest");
        Battlefield(P2, "Wall of Blossoms");
        
        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}