namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AetherSting
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal1DamageToOpponent()
      {
        Hand(P1, "Yavimaya Wurm");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Aether Sting");

        RunGame(1);

        Equal(P1.Life, 19);                
      }
      
    }
  }
}