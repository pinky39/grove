namespace Grove.Core.Zones
{
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;
  using Infrastructure;

  [Copyable]
  public abstract class OrderedZone : IEnumerable<Card>, IHashable
  {
    private readonly TrackableList<Card> _cards;

    protected OrderedZone(IEnumerable<Card> cards, ChangeTracker changeTracker)
    {
      _cards = new TrackableList<Card>(cards, changeTracker, orderImpactsHashcode: true);
    }

    protected OrderedZone(ChangeTracker changeTracker) : this(new Card[]{}, changeTracker) {}

    protected OrderedZone()
    {
      /* for state copy */
    }

    public int Count { get { return _cards.Count; } }

    public bool IsEmpty { get { return _cards.Count == 0; } }

    public abstract Zone Zone { get; }

    public IEnumerator<Card> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public virtual int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_cards);           
    }

    public virtual void Add(Card card)
    {
      _cards.Add(card);
      card.SetZone(Zone);
    }

    public virtual void Clear()
    {
      _cards.Clear();
    }

    public bool Contains(Card card)
    {
      return _cards.Contains(card);
    }   

    public void Hide()
    {
      foreach (var card in _cards)
      {
        card.Hide();
      }
    }

    public void Show()
    {
      foreach (var card in _cards)
      {
        card.Show();
      }
    }

    public virtual void Remove(Card card)
    {
      _cards.Remove(card);
    }

    public virtual void Shuffle()
    {
      _cards.Shuffle();
    }

    public override string ToString()
    {
      return string.Join(",", _cards.Select(
        card => card.ToString()));
    }
  }
}