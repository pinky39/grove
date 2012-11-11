namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class CreepingTarPit
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void Destroy()
      {
       
        var burst = C("Burst Lightning");
        var pit = C("Creeping Tar Pit");

        Hand(P2, burst);
        Battlefield(P2, "Mountain", "Mountain");
        Battlefield(P1, pit);

        Exec(
          At(Step.FirstMain)
            .Activate(pit, abilityIndex: 1),
          At(Step.DeclareAttackers)
            .DeclareAttackers(pit),          
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(pit).Zone);
              })
          );
      }      
    }
    
    
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Destroy()
      {
        var burst = C("Burst Lightning");
        var pit = C("Creeping Tar Pit");

        Hand(P2, burst);
        Battlefield(P1, pit);

        Exec(
          At(Step.FirstMain)
            .Activate(pit, abilityIndex: 1),
          At(Step.DeclareAttackers)
            .DeclareAttackers(pit),
          At(Step.DeclareBlockers)
            .Cast(burst, target: pit),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(pit).Zone))
          );
      }
    }
  }
}