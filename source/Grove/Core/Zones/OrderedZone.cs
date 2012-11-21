namespace Grove.Core.Zones
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Targeting;

  [Copyable]
  public abstract class OrderedZone : IEnumerable<Card>, IHashable, IZone
  {
    private readonly TrackableList<Card> _cards;

    protected OrderedZone(Player owner, Game game)
    {
      Owner = owner;
      _cards = new TrackableList<Card>(game.ChangeTracker, orderImpactsHashcode: true);
    }

    protected OrderedZone()
    {
      /* for state copy */
    }

    public Player Owner { get; private set; }

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

    public virtual void AfterAdd(Card card) {}
    public virtual void AfterRemove(Card card) {}

    void IZone.Remove(Card card)
    {
      Remove(card);
    }

    protected virtual void Remove(Card card)
    {
      _cards.Remove(card);
    }

    public virtual void Add(Card card)
    {
      _cards.Add(card);
      card.ChangeZone(this);
    }

    public virtual void AddToFront(Card card)
    {
      _cards.AddToFront(card);
      card.ChangeZone(this);
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
        card.UnHide();
      }
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

    public IEnumerable<ITarget> GenerateTargets(Func<Zone, Player, bool> zoneFilter)
    {
      if (zoneFilter(Zone, Owner))
      {
        foreach (var card in this)
        {
          yield return card;
        }
      }

      yield break;
    }
  }
}