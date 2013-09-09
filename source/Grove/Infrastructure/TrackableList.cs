namespace Grove.Infrastructure
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  [Copyable]
  public class TrackableList<T> : ITrackableCollection<T>, IList<T>, IHashable
  {
    private readonly List<T> _items = new List<T>();
    private readonly bool _orderImpactsHashcode;
    private INotifyChangeTracker _changeTracker = new ChangeTrackerInitializationGuard();
    private IHashDependancy _hashDependancy = new NoHashDependency();

    public TrackableList(IEnumerable<T> items, bool orderImpactsHashcode = false)
      : this(orderImpactsHashcode)
    {
      _items.AddRange(items);
    }

    public TrackableList(bool orderImpactsHashcode = false)
    {
      _orderImpactsHashcode = orderImpactsHashcode;
    }

    private TrackableList() {}

    public int CalculateHash(HashCalculator calc)
    {
      var hashcodes = new List<int>();

      foreach (var item in _items)
      {
        var hashable = item as IHashable;
        hashcodes.Add(hashable != null ? hashable.CalculateHash(calc) : item.GetHashCode());
      }

      if (_orderImpactsHashcode)
      {
        return HashCalculator.Combine(hashcodes);
      }

      return HashCalculator.CombineCommutative(hashcodes);
    }

    public T this[int index] { get { return _items[index]; } set { throw new NotSupportedException(); } }

    public void Insert(int index, T item)
    {
      _items.Insert(index, item);
      _changeTracker.NotifyValueAdded(this, item);
      _hashDependancy.InvalidateHash();
    }

    public void RemoveAt(int index)
    {
      var item = _items[index];
      Remove(item);
    }

    public int Count { get { return _items.Count; } }

    public bool IsReadOnly { get { return false; } }

    public void Add(T item)
    {      
      _items.Add(item);
      _changeTracker.NotifyValueAdded(this, item);
      _hashDependancy.InvalidateHash();
    }

    public void Clear()
    {
      _changeTracker.NotifyCollectionWillBeCleared(this);
      _items.Clear();
      _hashDependancy.InvalidateHash();
    }

    public bool Contains(T item)
    {
      return _items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      _items.CopyTo(array, arrayIndex);
    }

    public int IndexOf(T item)
    {
      return _items.IndexOf(item);
    }

    public bool Remove(T item)
    {
      _changeTracker.NotifyValueWillBeRemoved(this, item);
      _hashDependancy.InvalidateHash();
      return _items.Remove(item);
    }

    void ITrackableCollection<T>.AddWithoutTracking(T item)
    {
      _items.Add(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    void ITrackableCollection<T>.InsertWithoutTracking(T item, int index)
    {
      _items.Insert(index, item);
    }

    bool ITrackableCollection<T>.RemoveWithoutTracking(T item)
    {
      return _items.Remove(item);
    }

    public TrackableList<T> Initialize(INotifyChangeTracker changeTracker, IHashDependancy hashDependancy = null)
    {
      if (hashDependancy != null)
      {
        _hashDependancy = hashDependancy;
      }

      _changeTracker = changeTracker;
      return this;
    }

    public void AddToFront(T item)
    {
      _items.Insert(0, item);
      _changeTracker.NotifyValueAdded(this, item);
      _hashDependancy.InvalidateHash();
    }

    public T Pop()
    {
      var first = _items[0];
      Remove(first);
      return first;
    }

    public T PopLast()
    {
      var last = _items[Count - 1];
      Remove(last);
      return last;
    }

    public void Shuffle(IList<int> permutation)
    {
      var copy = _items.ToList();

      Clear();
      copy.ShuffleInPlace(permutation);

      foreach (var item in copy)
      {
        Add(item);
      }
    }

    public void ReorderFront(int[] permutation)
    {
      var head = _items
        .Take(permutation.Count())
        .ToList();

      foreach (var item in head)
      {
        Remove(item);
      }

      head.ShuffleInPlace(permutation);

      for (var i = head.Count - 1; i >= 0; i--)
      {
        AddToFront(head[i]);
      }
    }

    public void AddRange(IEnumerable<T> items)
    {
      foreach (var item in items)
      {
        _items.Add(item);
        _changeTracker.NotifyValueAdded(this, item);
      }

      _hashDependancy.InvalidateHash();
    }

    public void MoveToEnd(T item)
    {
      if (Remove(item) == false)
        return;

      Add(item);
    }

    public void MoveToFront(T item)
    {
      if (Remove(item) == false)
        return;

      AddToFront(item);
    }
  }
}