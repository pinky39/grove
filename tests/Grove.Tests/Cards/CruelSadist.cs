namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CruelSadist
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillElves()
      {
        var saddist = C("Cruel Sadist").AddCounters(2, CounterType.PowerToughness);
        
        Battlefield(P1, "Juggernaut", saddist, "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Llanowar Elves");

        P2.Life = 5;

        RunGame(1);

        Equal(1, C(saddist).CountersCount(CounterType.PowerToughness));
        Equal(0, P2.Life);
      }
    }
  }
}