namespace Grove.Core
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class CardPreviews : IEnumerable<Card>
  {
    private static readonly Random Rnd = new Random();
    private readonly CardDatabase _database;
    private Dictionary<string, Card> _cards = null;

    public CardPreviews(CardDatabase database)
    {
      _database = database;
    }

    public Card this[string name] { get { return _cards[name.ToLowerInvariant()]; } }

    public IEnumerator<Card> GetEnumerator()
    {
      return _cards.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Load()
    {
      if (_cards == null)
      {
        _cards = _database.CreatePreviewForEveryCard().ToDictionary(x => x.Name.ToLowerInvariant());
      }
    }

    public Card GetRandomPreview()
    {
      return this.ElementAt(Rnd.Next(0, _cards.Count));
    }
  }
}