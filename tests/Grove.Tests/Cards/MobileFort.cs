namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MobileFort
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Attack()
      {
        var fort = C("Mobile Fort");
        
        Battlefield(P1, fort, "Swamp", "Swamp", "Swamp");
        RunGame(2);

        Equal(17, P2.Life);
        True(C(fort).Has().Defender);        
      }
    }
  }
}