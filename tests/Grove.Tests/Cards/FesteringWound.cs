namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class FesteringWound
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantWall()
      {
        var wall = C("Wall of Blossoms");
        
        Hand(P1, "Festering Wound");
        Battlefield(P1, "Swamp", "Swamp");        
        Battlefield(P2, wall, "Llanowar Elves");
        P2.Life = 1;

        RunGame(4);

        Equal(0, P2.Life);
        True(C(wall).HasAttachments);
      }
    }
  }
}