namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Grindclock
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void MillTheOpponent()
      {
        Battlefield(P1, C("Grindclock").AddCounters(60, CounterType.Charge));
        
        RunGame(4);

        Equal(59, P2.Graveyard.Count);
        Equal(0, P2.Library.Count);        
      }
    }
  }
}