namespace Grove.Gameplay.Tournaments
{
  using System.Collections.Generic;
  using System.Linq;
  using Persistance;
  using UserInterface;

  public class PreConstructedLimitedDecks
  {
    private readonly DeckIo _deckIo;
    private Dictionary<int, List<Deck>> _decks;

    public PreConstructedLimitedDecks(DeckIo deckIo)
    {
      _deckIo = deckIo;
    }

    public void Load()
    {
      _decks = MediaLibrary.GetLimitedPreconstructedDecks()
        .Select(filename => _deckIo.Read(filename)).ToList()
        .Where(x => x.LimitedCode.HasValue)
        .GroupBy(x => x.LimitedCode).ToDictionary(x => x.Key.Value, x => x.ToList());
    }

    public IEnumerable<Deck> GetDecks(int limitedCode)
    {
      if (_decks.ContainsKey(limitedCode))
      {
        return _decks[limitedCode];
      }

      return Enumerable.Empty<Deck>();
    }
  }
}