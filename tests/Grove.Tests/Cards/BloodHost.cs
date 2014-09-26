namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class BloodHost
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackToKill()
      {
        Battlefield(P1, "Wall of Blossoms", "Blood Host", "Swamp", "Swamp");
        P2.Life = 4;

        RunGame(1);
        
        Equal(0, P2.Life);
        Equal(22, P1.Life);
      }
    }
  }
}