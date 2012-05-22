namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;
  using Infrastructure;

  [Copyable]
  public class Deck : IEnumerable<Card>
  {
    private readonly List<Card> _cards = new List<Card>();

    public Deck(IEnumerable<Card> cards)
    {
      _cards.AddRange(cards);
    }

    public int Count
    {
      get { return _cards.Count; }
    }
    
    public IEnumerator<Card> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}