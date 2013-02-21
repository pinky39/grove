namespace Grove.Tests
{
  using System;
  using Core;
  using Ui;
  using Xunit;

  public class MatchSimulatorFacts
  {
    //[Fact]
    public void Simulate()
    {
      var deck1 = "Kuno-rg-beastfires";
      var deck2 = "Kuno-bu-control";
      
      var result =
        MatchSimulator.Simulate(GetDeck(deck1), GetDeck(deck2));

      Console.WriteLine(@"{0} vs {1}", deck1, deck2);
      Console.WriteLine(@"Match duration: {0}.", result.Duration);
      Console.WriteLine(@"{0} win count: {1}.", deck1, result.Deck1WinCount);
      Console.WriteLine(@"{0} win count: {1}.", deck2, result.Deck2WinCount);

      Assert.True(result.Deck1WinCount + result.Deck2WinCount >= 2);
    }

    private static CardDatabase _cardDatabase;

    private readonly IoC _container = IoC.Test();
    private MatchSimulator MatchSimulator { get { return _container.Resolve<MatchSimulator>(); } }
    private CardDatabase CardDatabase { get { return _cardDatabase ?? (_cardDatabase = _container.Resolve<CardDatabase>().LoadPreviews()); } }

    private Deck GetDeck(string name)
    {
      return MediaLibrary.GetDeck(name, CardDatabase);
    }
  }
}