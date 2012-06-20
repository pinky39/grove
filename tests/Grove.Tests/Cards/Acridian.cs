namespace Grove.Tests.Cards
{
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class Acridian
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PayEcho()
      {
        var acridian = C("Acridian");
        var forest1 = C("Forest");
        var forest2 = C("Forest");

        Hand(P1, acridian);        
        Battlefield(P1, forest1, forest2);

        RunGame(3);

        Equal(Zone.Battlefield, C(acridian).Zone);
        True(C(forest1).IsTapped);
        True(C(forest2).IsTapped);
      }
    }
  }
}