namespace Grove.Gameplay.Zones
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Misc;

  [Serializable]
  public abstract class UnorderedZone : GameObject, IEnumerable<Card>, IHashable, IZone
  {
    private readonly TrackableList<Card> _cards = new TrackableList<Card>();

    protected UnorderedZone(Player owner)
    {
      Owner = owner;
    }

    protected UnorderedZone()
    {
      /* for state copy */
    }

    public Player Owner { get; private set; }
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

    public virtual void AfterAdd(Card card) {}

    public virtual void AfterRemove(Card card) {}

    public abstract Zone Zone { get; }

    void IZone.Remove(Card card)
    {
      Remove(card);
    }

    public virtual void Initialize(Game game)
    {
      Game = game;
      _cards.Initialize(ChangeTracker);
    }

    public event EventHandler<ZoneChangedEventArgs> CardAdded = delegate { };
    public event EventHandler<ZoneChangedEventArgs> CardRemoved = delegate { };

    public virtual IEnumerable<Card> GenerateZoneTargets(Func<Zone, Player, bool> zoneFilter)
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

    public virtual void Add(Card card)
    {
      _cards.Add(card);
      card.ChangeZone(this);

      CardAdded(this, new ZoneChangedEventArgs(card));
    }

    protected virtual void Remove(Card card)
    {
      _cards.Remove(card);
      CardRemoved(this, new ZoneChangedEventArgs(card));
    }

    public override string ToString()
    {
      return string.Join(",", _cards.Select(
        card => card.ToString()));
    }
  }
}