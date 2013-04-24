namespace Grove.Tests.Cards
{
  using Core;
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Endoskeleton
  {    
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void LeaveTapped()
      {
        var kavu = C("Flametongue Kavu");
        var endo = C("Endoskeleton");
        var knight = C("White Knight");
        var forest = C("Forest");

        Battlefield(P1, knight);
        Battlefield(P2, kavu, endo, forest, "Forest");

        // only works with low life, otherwise ai will not
        // consider blocking since it only checks 1 scenario
        // when blocking is improoved this is no longer necessary
        P2.Life = 2;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(knight),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(5, C(kavu).Toughness);
                Equal(Zone.Graveyard, C(knight).Zone);
              }),
          At(Step.Upkeep, 2)
            .Verify(() =>
              {
                Equal(5, C(kavu).Toughness);
                False(C(forest).IsTapped);
              })
        );
        
      }
    }
    
    
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void TapUntap()
      {
        var bear = C("Grizzly Bears");
        var endo = C("Endoskeleton");

        Battlefield(P1, bear, endo);

        Exec(
          At(Step.FirstMain)
            .Activate(endo, target: bear)
            .Verify(() => Equal(5, C(bear).Toughness)),
          At(Step.Untap, 3)
            .DoNotUntap(),
          At(Step.FirstMain, 3)
            .Verify(() => Equal(5, C(bear).Toughness)),
          At(Step.Untap, 5)
            .Untap(),
          At(Step.FirstMain, 5)
            .Verify(() => Equal(2, C(bear).Toughness))
        );
      }
    }
  }
}