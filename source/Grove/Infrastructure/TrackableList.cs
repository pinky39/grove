namespace Grove.Infrastructure
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  
  [Copyable]
  public class TrackableList<T> : ITrackableCollection<T>, IList<T>, IHashable
  {
    private readonly ChangeTracker _changeTracker;
    private readonly IHashDependancy _hashDependancy;
    private readonly List<T> _items = new List<T>();        
    private readonly bool _orderImpactsHashcode;

    public TrackableList(IEnumerable<T> items, ChangeTracker changeTracker, IHashDependancy hashDependancy = null,
                         bool orderImpactsHashcode = false)
      : this(changeTracker, hashDependancy, orderImpactsHashcode)
    {
      _items.AddRange(items);
    }

    public TrackableList(ChangeTracker changeTracker, IHashDependancy hashDependancy = null,
                         bool orderImpactsHashcode = false)
    {
      _changeTracker = changeTracker;
      _orderImpactsHashcode = orderImpactsHashcode;
      _hashDependancy = hashDependancy ?? new NullHashDependency();
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

    void IList<T>.Insert(int index, T item)
    {
      throw new NotImplementedException();
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

    public void AddToFront(T item)
    {
      _items.Insert(0, item);
      _changeTracker.NotifyValueAdded(this, item);
      _hashDependancy.InvalidateHash();
    }

    void ITrackableCollection<T>.AddWithoutTracking(T item)
    {
      _items.Add(item);
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

    public IEnumerator<T> GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int IndexOf(T item)
    {
      return _items.IndexOf(item);
    }

    void ITrackableCollection<T>.InsertWithoutTracking(int index, T item)
    {
      _items.Insert(index, item);
    }

    public bool Remove(T item)
    {
      _changeTracker.NotifyValueWillBeRemoved(this, item);
      _hashDependancy.InvalidateHash();
      return _items.Remove(item);
    }

    bool ITrackableCollection<T>.RemoveWithoutTracking(T item)
    {
      return _items.Remove(item);
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

    public void Shuffle()
    {
      var permutation = RandomEx.Permutation(0, Count);
      var copy = _items.ToList();

      Clear();
      copy.ShuffleInPlace(permutation);

      foreach (var item in copy)
      {
        Add(item);
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
  }
}