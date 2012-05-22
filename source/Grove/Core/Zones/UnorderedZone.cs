namespace Grove.Core.Zones
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public abstract class UnorderedZone : IEnumerable<Card>, IHashable
  {
    private readonly TrackableList<Card> _cards;    

    protected UnorderedZone(ChangeTracker changeTracker)
    {
      _cards = new TrackableList<Card>(changeTracker);      
    }

    protected UnorderedZone()
    {
      /* for state copy */
    }

    public int Count { get { return _cards.Count; } }
    public IEnumerable<Card> Creatures { get { return _cards.Where(card => card.Is().Creature); } }

    public bool IsEmpty { get { return _cards.Count == 0; } }
    public IEnumerable<Card> Lands { get { return _cards.Where(card => card.Is().Land); } }

    public Card RandomCard
    {
      get
      {
        var randomIndex = RandomEx.Next(0, _cards.Count - 1);
        return _cards.ElementAt(randomIndex);
      }
    }

    public abstract Zone Zone { get; }

    public IEnumerator<Card> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public virtual int CalculateHash(HashCalculator hashCalculator)
    {      
      return hashCalculator.Calculate(_cards);
    }

    public virtual void Add(Card card)
    {
      _cards.Add(card);
      card.SetZone(Zone);
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

    public override string ToString()
    {
      return string.Join(",", _cards.Select(
        card => card.ToString()));
    }

    protected virtual void Clear()
    {
      _cards.Clear();
    }
  }
}