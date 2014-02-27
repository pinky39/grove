namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PendrellFlux
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void ResurectedCreatureIsNoLongerInfluencedByFlux()
      {
        var flux = C("Pendrell Flux");
        var shock = C("Shock");
        var bear = C("Grizzly Bears");
        var exhume = C("Exhume");
        var forest = C("Forest");
        var mountain = C("Mountain");
        
        Hand(P1, flux);
        Hand(P2, shock, exhume);

        Battlefield(P2, bear, forest, mountain);
       

        Exec(
          At(Step.FirstMain)
            .Cast(flux, target: bear),
          At(Step.FirstMain, turn: 2)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(bear).Zone);
                True(C(forest).IsTapped);
                True(C(mountain).IsTapped);
              }),
          At(Step.SecondMain, turn: 2)
            .Cast(shock, target: bear)
            .Cast(exhume),
          At(Step.FirstMain, turn: 4)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(bear).Zone);
                False(C(forest).IsTapped);
                False(C(mountain).IsTapped);
              })
          );
      }
    }
    
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantForce()
      {
        var force = C("Verdant Force");

        Hand(P1, "Pendrell Flux");
        Battlefield(P1, "Island", "Island");
        Battlefield(P2, force);

        RunGame(2);

        Equal(Zone.Graveyard, C(force).Zone);
      }
    }
  }
}