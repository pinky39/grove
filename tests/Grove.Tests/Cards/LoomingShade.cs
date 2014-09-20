namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class LoomingShade
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void BlockAndPump()
      {
        var bears = C("Grizzly Bears");
        var loomingShade = C("Looming Shade");

        Battlefield(P1, bears);
        Battlefield(P2, loomingShade, "Swamp", "Swamp");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bears),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Graveyard, C(bears).Zone);
                Equal(Zone.Battlefield, C(loomingShade).Zone);
              }));
      }

      [Fact]
      public void DoNotBlockShade()
      {
        var bears = C("Grizzly Bears");
        var loomingShade = C("Looming Shade");

        Battlefield(P1, loomingShade, "Swamp", "Swamp");
        Battlefield(P2, bears);
        
        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(loomingShade),
          At(Step.DeclareBlockers)
            .Activate(loomingShade)
            .Activate(loomingShade),
          At(Step.SecondMain)
            .Verify(() =>
            {
              Equal(17, P2.Life);
              Equal(Zone.Battlefield, C(bears).Zone);
              Equal(Zone.Battlefield, C(loomingShade).Zone);
            }));
      }

      
    }

    public class Ai : AiScenario
    {
      [Fact]
      public void AttackWithShade()
      {
        var shade = C("Looming Shade");
        Battlefield(P1, shade, "Llanowar Elves", "Llanowar Elves", "Llanowar Elves", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        True(C(shade).IsTapped);
      }
    }
  }
}