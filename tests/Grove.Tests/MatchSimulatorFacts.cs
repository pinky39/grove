namespace Grove.Tests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Ui;
  using Xunit;

  public class MatchSimulatorFacts : IDisposable
  {
    protected static readonly IoC Container = IoC.Test();
    private readonly IDisposable _scope;

    protected MatchSimulator MatchSimulator {get { return Container.Resolve<MatchSimulator>(); }}

    public MatchSimulatorFacts()
    {
      _scope = Container.BeginScope();
    }
    
    [Fact]
    public void Simulate()
    {
      var deck1 = "Kuno-rg-beastfires";
      var deck2 = "Kuno-bu-control";

      var result = 
        MatchSimulator.Simulate(
          GetDeck(deck1), 
          GetDeck(deck2));

      Console.WriteLine(@"{0} vs {1}", deck1, deck2);
      Console.WriteLine(@"Match duration: {0}.", result.Duration);      
      Console.WriteLine(@"{0} win count: {1}.", deck1, result.Deck1WinCount);
      Console.WriteLine(@"{0} win count: {1}.", deck2, result.Deck2WinCount);
                        
      Assert.True(result.Deck1WinCount + result.Deck2WinCount >= 2);
    }

    private static List<string> GetDeck(string name)
    {
      return DeckFile.Read(MediaLibrary.GetDeck(name)).AllCards.ToList();
    }

    public void Dispose()
    {
      if (_scope != null)
        _scope.Dispose();
    }
  }
}