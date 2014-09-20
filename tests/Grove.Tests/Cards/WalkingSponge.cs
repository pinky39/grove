namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WalkingSponge
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackWithDragonToWin()
      {
        Battlefield(P1, "Shivan Dragon", "Walking Sponge");
        Battlefield(P2, "Angelic Wall");
        
        P2.Life = 5;
        
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}