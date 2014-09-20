namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class AvalancheRiders
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyLandDeal2Damage()
      {
        Hand(P1, "Avalanche Riders");
        Battlefield(P1, "Mountain", "Mountain", "Forest", "Forest");
        Battlefield(P2, "Mountain");
        
        RunGame(1);

        Equal(0, P2.Battlefield.Count(c => c.Is().Land));
        Equal(18, P2.Life);
      }
    }
  }
}