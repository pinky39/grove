namespace Grove.Infrastructure
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  [Copyable]
  public class TrackableSet<T> : ITrackableCollection<T>, IHashable
  {
    private readonly Dictionary<T, int> _items = new Dictionary<T, int>();
    private INotifyChangeTracker _changeTracker = new ChangeTracker.Guard();
    private IHashDependancy _hashDependancy = new NoHashDependency();

    public TrackableSet(IEnumerable<T> items)
    {
      foreach (var item in items)
      {
        _items.Add(item, 1);
      }
    }

    public TrackableSet() {}

    public int Count { get { return _items.Count; } }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_items.Keys.ToList(),
        orderImpactsHashcode: false);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return _items.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    void ITrackableCollection<T>.AddWithoutTracking(T item)
    {
      _items.Add(item, 1);
    }

    bool ITrackableCollection<T>.RemoveWithoutTracking(T item)
    {
      return _items.Remove(item);
    }

    void ITrackableCollection<T>.InsertWithoutTracking(T item, int index)
    {
      throw new NotImplementedException();
    }


    public void Add(T item)
    {
      _items.Add(item, 1);
      _changeTracker.NotifyValueAdded(this, item);
      _hashDependancy.InvalidateHash();
    }

    public bool Remove(T item)
    {
      _changeTracker.NotifyValueWillBeRemoved(this, item);
      _hashDependancy.InvalidateHash();
      return _items.Remove(item);
    }

    public void Clear()
    {
      _changeTracker.NotifyCollectionWillBeCleared(this);
      _items.Clear();
      _hashDependancy.InvalidateHash();
    }

    public bool Contains(T item)
    {
      return _items.ContainsKey(item);
    }

    public TrackableSet<T> Initialize(INotifyChangeTracker changeTracker, IHashDependancy hashDependancy = null)
    {
      if (hashDependancy != null)
      {
        _hashDependancy = hashDependancy;
      }

      _changeTracker = changeTracker;
      return this;
    }

    public void AddRange(IEnumerable<T> items)
    {
      foreach (var item in items)
      {
        _items.Add(item, 1);
        _changeTracker.NotifyValueAdded(this, item);
      }

      _hashDependancy.InvalidateHash();
    }
  }
}