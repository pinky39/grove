namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class StabWound
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantedCreatureControllerLooses2Life()
      {
        Hand(P1, "Stab Wound");
        Battlefield(P1, "Swamp", "Mountain", "Mountain");

        var wall = C("Wall of Fire");
        Battlefield(P2, wall);

        RunGame(2);
        
        Equal(18, P2.Life);
        Equal(3, C(wall).Toughness);
      }
    }
  }
}