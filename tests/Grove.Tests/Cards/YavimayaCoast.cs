namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class YavimayaCoast
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AddGreenManaToPoolToPlayWall()
      {
        var wall = C("Wall of Blossoms");
        var coast = C("Yavimaya Coast");
        
        Hand(P1, wall);        
        Battlefield(P1, coast, "Mountain");

        RunGame(1);
        
        Equal(Zone.Battlefield, C(wall).Zone);
        Equal(19, P1.Life);
        True(C(coast).IsTapped);
      }
    }
  }
}