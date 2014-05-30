  namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Sporogenesis
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutCounterOnBear()
      {
        var bears = C("Grizzly Bears");
        Battlefield(P1, bears, "Sporogenesis");        
                
        RunGame(1);

        Equal(1, C(bears).CountersCount(CounterType.Fungus));
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PutASaprolingInPlayForEachCounter()
      {
        var bear = C("Grizzly Bears");
        var shock = C("Shock");
        
        Battlefield(P1, "Sporogenesis");        
        Battlefield(P1, bear);
        Hand(P2, shock);

        Exec(
          At(Step.Upkeep, turn: 1)
            .Target(bear),
          At(Step.Upkeep, turn: 3)
            .Target(bear),
          At(Step.FirstMain, turn: 3)
            .Cast(shock, target: bear),
          At(Step.SecondMain, turn: 3)
            .Verify(() => Equal(2, P1.Battlefield.Count(c => c.Is().Token))
          ));
      }

      [Fact]
      public void RemoveAllCounters()
      {
        var bear = C("Grizzly Bears");
        var disenchant = C("Disenchant");
        var sporogenesis = C("Sporogenesis");
        
        Battlefield(P1, sporogenesis);        
        Battlefield(P1, bear);
        Hand(P2, disenchant);

        Exec(
          At(Step.Upkeep, turn: 1)
            .Target(bear),
          At(Step.Upkeep, turn: 3)
            .Target(bear),
          At(Step.FirstMain, turn: 3)
            .Cast(disenchant, target: sporogenesis),
          At(Step.SecondMain, turn: 3)
            .Verify(() => Equal(0, C(bear).Counters)
          ));
      }
    }
  }
}