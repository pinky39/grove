namespace Grove
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;

  [Copyable]
  public abstract class OrderedZone : GameObject, IEnumerable<Card>, IHashable, IZone
  {
    private readonly TrackableList<Card> _cards = new TrackableList<Card>(orderImpactsHashcode: true);

    protected OrderedZone(Player owner)
    {
      Owner = owner;
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

    public abstract Zone Name { get; }

    public virtual void AfterAdd(Card card) {}

    public virtual void AfterRemove(Card card) {}

    void IZone.Remove(Card card)
    {
      Remove(card);
    }

    public event EventHandler Shuffled = delegate { };

    public event EventHandler<ZoneChangedEventArgs> CardAdded = delegate { };
    public event EventHandler<ZoneChangedEventArgs> CardRemoved = delegate { };

    public virtual void Initialize(Game game)
    {
      Game = game;
      _cards.Initialize(ChangeTracker);
    }

    protected virtual void Remove(Card card)
    {
      _cards.Remove(card);
      CardRemoved(this, new ZoneChangedEventArgs(card));
    }

    public virtual void AddToEnd(Card card)
    {
      Add(card, c => _cards.Add(c));
    }

    private void Add(Card card, Action<Card> add)
    {      
      card.ChangeZone(
        destination: this,
        add: add);

      CardAdded(this, new ZoneChangedEventArgs(card));
    }

    public virtual void AddToFront(Card card)
    {
      Add(card, c => _cards.AddToFront(c));
    }

    public virtual void AddToFront(Card card, int skipCount)
    {
      if (skipCount > _cards.Count)
        skipCount = _cards.Count;

      Add(card, c => _cards.Insert(skipCount, c));
    }

    protected virtual void MoveToEnd(Card card)
    {
      _cards.MoveToEnd(card);
    }

    protected virtual void MoveToFront(Card card)
    {
      _cards.MoveToFront(card);
    }

    public bool Contains(Card card)
    {
      return _cards.Contains(card);
    }

    public virtual void Shuffle()
    {
      _cards.Shuffle(GetRandomPermutation(0, _cards.Count));

      foreach (var card in _cards)
      {
        card.ResetVisibility();
      }

      Shuffled(this, EventArgs.Empty);
    }


    public virtual void ReorderFront(int[] permutation)
    {
      _cards.ReorderFront(permutation);

      Shuffled(this, EventArgs.Empty);
    }

    public override string ToString()
    {
      return string.Join(",", _cards.Select(
        card => card.ToString()));
    }

    public void GenerateZoneTargets(Func<Zone, Player, bool> zoneFilter, List<ITarget> targets)
    {
      if (zoneFilter(Name, Owner))
      {
        targets.AddRange(_cards);
      }
    }
  }
}