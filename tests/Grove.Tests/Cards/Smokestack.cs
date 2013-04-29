namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Smokestack
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void P2SacsAcidicSlime()
      {        
        Battlefield(P1, "Smokestack", "Verdant Force");        
        Battlefield(P2, "Acidic Slime");

        P1.Life = 2;
        P2.Life = 7;
        
        RunGame(3);

        Equal(2, P1.Life);
        Equal(-1, P2.Life);
      }

    }
  }
}