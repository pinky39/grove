namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SteamVents
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PayLife()
      {
        Hand(P1, "Steam Vents", "Grizzly Bears");
        Battlefield(P1, "Forest");
        RunGame(1);

        
        Equal(3, P1.Battlefield.Count());        
        Equal(18, P1.Life);
        
      }
    }
  }
}