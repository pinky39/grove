namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;
  using System.Linq;

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
    
    public class Predefined : PredefinedAiScenario
    {
      [Fact]
      public void EchoAfterReanimate()
      {
        var acridian = C("Acridian");
        var unearth = C("Unearth");
        var beacon = C("Beacon of Destruction");
        
        Hand(P1, acridian, unearth);
        Battlefield(P1, "Forest", "Forest", "Swamp");        
        Hand(P2, beacon);

        Exec(
          At(Step.FirstMain, turn: 1)
            .Cast(acridian),
          At(Step.FirstMain, turn: 3)
            .Cast(beacon, target: acridian)          
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(acridian).Zone);
              }),
          At(Step.SecondMain, turn: 3)
            .Cast(unearth, target: acridian),
          At(Step.FirstMain, turn: 5)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(acridian).Zone);
                Equal(2, P1.Battlefield.Count(x => x.Is().Land && x.IsTapped));
              })        
        );
      }
    }
  }
}