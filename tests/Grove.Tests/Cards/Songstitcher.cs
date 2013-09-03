namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Songstitcher
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PreventDamage()
      {
        var dragon = C("Shivan Dragon");
        var titcher = C("Songstitcher");

        Battlefield(P1, dragon);
        Battlefield(P2, titcher, "Plains", "Plains");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dragon),
          At(Step.DeclareBlockers)
            .Activate(titcher, target: dragon),
          At(Step.SecondMain)
            .Verify(() => Equal(20, P2.Life))
          );
      }
    }

    
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void PreventDamage()
      {
        var dragon = C("Shivan Dragon");
        var titcher = C("Songstitcher");

        Battlefield(P1, dragon);
        Battlefield(P2, titcher, "Plains", "Plains");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dragon),
          At(Step.SecondMain)
            .Verify(() => Equal(20, P2.Life))
          );
      }

      [Fact]
      public void PreferActivatingSticher()
      {
        var dragon = C("Shivan Dragon");
        var titcher = C("Songstitcher");
        var bears = C("Grizzly Bears");

        Battlefield(P1, dragon);        
        Battlefield(P2, titcher, "Plains", "Plains", bears, "Martyr's Cause");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dragon),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Battlefield, C(bears).Zone);
              })
          );
      }
    }
  }
}