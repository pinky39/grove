namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BrineSeer
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void CounterBear()
      {
        var bears = C("Grizzly Bears");
        var brineSeer = C("Brine Seer");

        Hand(P1, bears);
        Hand(P2, "Brine Seer");

        Battlefield(P1, "Grizzly Bears");      
        Battlefield(P2, brineSeer, "Island", "Island", "Island");          

        Exec(
          At(Step.FirstMain)
          .Cast(bears)
          .Verify(() =>
            {              
              False(C(brineSeer).HasSummoningSickness);
              Equal(Zone.Graveyard, C(bears).Zone);
            })
          );
        
      }
    }
  }
}