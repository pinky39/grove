namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AuraThief
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainControlOfAllEnchantments()
      {
        var wall = C("Wall of Blossoms");
        var anaconda = C("Anaconda");

        Battlefield(P1, "Aura Thief", wall);        
        Battlefield(P2, "Mountain", "Glorious Anthem", anaconda.IsEnchantedWith("Rancor"));        
        Hand(P2, "Shock");
        P2.Life = 2;

        RunGame(1);

        Equal(5, C(anaconda).Power);
        Equal(1, C(wall).Power);        
      }
    }
  }
}