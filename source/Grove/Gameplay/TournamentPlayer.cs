namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using Artifical;

  public class TournamentPlayer
  {
    private readonly List<string> _library;
    private List<string> _deck;

    public TournamentPlayer(string name, List<string> library)
    {
      _library = library;
      Name = name;
    }

    public string Name { get; private set; }
    public IEnumerable<string> Deck { get { return _deck; } }

    public void GenerateDeck(DeckBuilder deckBuilder, CardRatings cardRatings)
    {
      _deck = deckBuilder.BuildDeck(_library, cardRatings);
    }
  }
}