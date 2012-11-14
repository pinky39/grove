namespace Grove.Tests
{
  using System;
  using Core;
  using Ui;
  using Xunit;

  public class MatchSimulatorFacts
  {
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

    private readonly IoC _container = IoC.Test();
    protected MatchSimulator MatchSimulator { get { return _container.Resolve<MatchSimulator>(); } }
    protected CardDatabase CardDatabase { get { return _container.Resolve<CardDatabase>(); } }

    private Deck GetDeck(string name)
    {
      return MediaLibrary.GetDeck(name, CardDatabase);
    }
  }
}