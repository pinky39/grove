namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ParasiticBond
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantForce()
      {
        Hand(P1, "Parasitic Bond");
        
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Verdant Force");

        P2.Life = 4;

        RunGame(3);

        Equal(2, P2.Life);
      }
    }
  }
}