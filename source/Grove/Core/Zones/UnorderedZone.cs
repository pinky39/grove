namespace Grove.Core.Zones
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Messages;

  [Copyable]
  public abstract class UnorderedZone : IEnumerable<Card>, IHashable, IZone
  {
    private readonly TrackableList<Card> _cards;
    private readonly Publisher _publisher;

    protected UnorderedZone(Game game)
    {
      _publisher = game.Publisher;
      _cards = new TrackableList<Card>(game.ChangeTracker);
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

    public abstract Zone Zone { get; }

    void IZone.Remove(Card card)
    {
      Remove(card);
    }

    protected virtual void AfterAdd(Card card) {}
    protected virtual void AfterRemove(Card card) {}

    public virtual void Add(Card card)
    {
      var oldZone = card.Zone;

      card.ChangeZoneTo(this);
      _cards.Add(card);

      AfterAdd(card);

      _publisher.Publish(new CardChangedZone
        {
          Card = card,
          From = oldZone,
          To = Zone
        });
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


    protected virtual void Remove(Card card)
    {
      _cards.Remove(card);
    }

    public override string ToString()
    {
      return string.Join(",", _cards.Select(
        card => card.ToString()));
    }
  }
}