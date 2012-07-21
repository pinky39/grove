namespace Grove.Core.Zones
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Messages;

  [Copyable]
  public abstract class OrderedZone : IEnumerable<Card>, IHashable, IZone
  {
    private readonly TrackableList<Card> _cards;
    private readonly Publisher _publisher;

    protected OrderedZone(Game game)
    {      
      _cards = new TrackableList<Card>(game.ChangeTracker, orderImpactsHashcode: true);
      _publisher = game.Publisher;
    }    

    protected OrderedZone()
    {
      /* for state copy */
    }

    public int Count { get { return _cards.Count; } }

    public bool IsEmpty { get { return _cards.Count == 0; } }

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

    void IZone.Remove(Card card, bool moveToOpponentZone)
    {
      Remove(card, moveToOpponentZone);
    }

    public virtual void AddToFront(Card card)
    {
      card.ChangeZoneTo(this);
      _cards.AddToFront(card);

      AfterAdd(card);
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


    protected virtual void Remove(Card card, bool moveToOpponentZone)
    {
      _cards.Remove(card);
      
      if (moveToOpponentZone)
        return;
      
      AfterRemove(card);
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