namespace Grove.Infrastructure
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Remoting.Messaging;

  public interface ITrackableCollection<T> : IEnumerable<T>
  {
    void AddWithoutTracking(T item);
    bool RemoveWithoutTracking(T item);
    void InsertWithoutTracking(T item, int index);
  }

  public interface INotifyChangeTracker
  {
    void NotifyCollectionWillBeCleared<T>(ITrackableCollection<T> trackableCollection);
    void NotifyValueAdded<T>(ITrackableCollection<T> trackableCollection, T added);
    void NotifyValueChanged<T>(ITrackableValue<T> trackableValue);
    void NotifyValueWillBeRemoved<T>(ITrackableCollection<T> trackableCollection, T removed);
  }

  public interface ITrackableValue<T> : IHashable
  {
    T Value { get; set; }
  }

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

    public TrackableSet()
    {
    }

    public int Count
    {
      get { return _items.Count; }
    }

    public int CalculateHash(HashCalculator calc)
    {
      var hashcodes = new List<int>();

      foreach (var item in _items.Keys)
      {
        var hashable = item as IHashable;
        hashcodes.Add(hashable != null ? hashable.CalculateHash(calc) : item.GetHashCode());
      }

      return HashCalculator.CombineCommutative(hashcodes);
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

  [Copyable]
  public class TrackableList<T> : ITrackableCollection<T>, IList<T>, IHashable
  {
    private readonly List<T> _items = new List<T>();
    private readonly bool _orderImpactsHashcode;
    private INotifyChangeTracker _changeTracker = new ChangeTracker.Guard();
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

    private TrackableList()
    {
    }

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

    public T this[int index]
    {
      get { return _items[index]; }
      set { throw new NotSupportedException(); }
    }

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

    public int Count
    {
      get { return _items.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

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
      var removed = Remove(item);
      Asrt.True(removed, "Item must be present to move it.");
      Add(item);
    }

    public void MoveToFront(T item)
    {
      var removed = Remove(item);
      Asrt.True(removed, "Item must be present to move it.");
      AddToFront(item);
    }
  }

  public class TrackableEvent : ICopyable
  {
    private INotifyChangeTracker _changeTracker;
    private TrackableList<EventHandler> _handlers = new TrackableList<EventHandler>();
    private object _sender;

    private TrackableEvent()
    {
    }

    public TrackableEvent(object sender)
    {
      _sender = sender;
    }

    public void Copy(object original, CopyService copyService)
    {
      var org = (TrackableEvent) original;

      // create a copy without handlers
      _sender = copyService.Copy(org._sender);
      _changeTracker = copyService.Copy(org._changeTracker);
      _handlers = new TrackableList<EventHandler>();
      _handlers.Initialize(_changeTracker);
    }

    public TrackableEvent Initialize(INotifyChangeTracker changeTracker)
    {
      _changeTracker = changeTracker;
      _handlers.Initialize(changeTracker);
      return this;
    }

    public void Raise()
    {
      foreach (var eventHandler in _handlers.ToArray())
      {
        eventHandler(_sender, EventArgs.Empty);
      }
    }

    public void Add(EventHandler eventHandler)
    {
      _handlers.Add(eventHandler);
    }

    public void Remove(EventHandler eventHandler)
    {
      _handlers.Remove(eventHandler);
    }

    public static TrackableEvent operator +(TrackableEvent trackableEvent, EventHandler eventHandler)
    {
      trackableEvent.Add(eventHandler);
      return trackableEvent;
    }

    public static TrackableEvent operator -(TrackableEvent trackableEvent, EventHandler eventHandler)
    {
      trackableEvent.Remove(eventHandler);
      return trackableEvent;
    }
  }

  [Copyable]
  public class Trackable<T> : ITrackableValue<T>
  {
    private INotifyChangeTracker _changeTracker = new ChangeTracker.Guard();
    private IHashDependancy _hashDependency = new NoHashDependency();
    private T _value;

    public Trackable() : this(default(T))
    {
    }

    public Trackable(T value)
    {
      _value = value;
    }

    public T Value
    {
      get { return _value; }
      set
      {
        if (Equals(value, _value))
          return;

        _changeTracker.NotifyValueChanged(this);
        _hashDependency.InvalidateHash();
        _value = value;
      }
    }

    T ITrackableValue<T>.Value
    {
      get { return _value; }
      set { _value = value; }
    }

    public int CalculateHash(HashCalculator calc)
    {
      var hashable = _value as IHashable;

      return hashable != null
        ? calc.Calculate(hashable)
        : _value.GetHashCode();
    }

    public Trackable<T> Initialize(INotifyChangeTracker changeTracker, IHashDependancy hashDependancy = null)
    {
      _changeTracker = changeTracker;

      if (hashDependancy != null)
        _hashDependency = hashDependancy;

      return this;
    }

    public void Rollback(object value)
    {
      _value = (T) value;
    }

    public override string ToString()
    {
      return _value.ToString();
    }

    public static implicit operator T(Trackable<T> trackable)
    {
      return trackable.Value;
    }
  }

  public class ChangeTracker : ICopyable, INotifyChangeTracker
  {
    private readonly Stack<Action> _changeHistory = new Stack<Action>();
    private bool _isEnabled;
    private bool _isLocked;

    public void Copy(object original, CopyService copyService)
    {
      var org = (ChangeTracker) original;
      _isEnabled = org._isEnabled;
    }

    public void NotifyCollectionWillBeCleared<T>(ITrackableCollection<T> trackableCollection)
    {
      AssertNotLocked();

      if (!_isEnabled)
      {
        return;
      }

      var elements = trackableCollection.ToList();

      if (elements.Count == 0)
        return;

      _changeHistory.Push(() =>
        {
          foreach (var element in elements)
          {
            trackableCollection.AddWithoutTracking(element);
          }
        });
    }

    public void NotifyValueAdded<T>(ITrackableCollection<T> trackableCollection, T added)
    {
      AssertNotLocked();

      if (!_isEnabled)
      {
        return;
      }

      _changeHistory.Push(() => trackableCollection.RemoveWithoutTracking(added));
    }

    public void NotifyValueChanged<T>(ITrackableValue<T> trackableValue)
    {
      AssertNotLocked();

      if (!_isEnabled)
      {
        return;
      }

      var value = trackableValue.Value;
      _changeHistory.Push(() => trackableValue.Value = value);
    }

    public void NotifyValueWillBeRemoved<T>(ITrackableCollection<T> trackableCollection, T removed)
    {
      AssertNotLocked();

      if (!_isEnabled)
      {
        return;
      }

      var list = trackableCollection as IList<T>;

      if (list != null)
      {
        var index = list.IndexOf(removed);

        if (index == -1)
          return;

        _changeHistory.Push(() => trackableCollection.InsertWithoutTracking(removed, index));

        return;
      }

      if (trackableCollection.Contains(removed))
      {
        _changeHistory.Push(() => trackableCollection.AddWithoutTracking(removed));
      }
    }

    public Snapshot CreateSnapshot()
    {
      LogFile.Debug("Create snapshot.");
      Asrt.True(_isEnabled, "Tracker is disabled, did you forget to enable it?");

      return new Snapshot(_changeHistory.Count);
    }

    public void Disable()
    {
      Asrt.True(_changeHistory.Count == 0,
        String.Format(
          "Disabling a change tracker with history ({0}) is not allowed. This is a common indication of an incorrect object copy.",
          _changeHistory.Count));

      _isEnabled = false;
      _changeHistory.Clear();
      Guard.Disable();
    }

    public void Enable()
    {
      _isEnabled = true;
      Guard.Enable();
    }

    private void AssertNotLocked()
    {
      Asrt.True(!_isLocked,
        "Trying to modify a locked change tracker. This is a common indication of an incorrect object copy.");
    }

    public void RollbackToSnapshot(Snapshot snapshot)
    {
      LogFile.Debug("Restore from snapshot.");
      Asrt.True(_isEnabled, "Tracker is disabled, did you forget to enable it?");

      while (_changeHistory.Count > snapshot.History)
      {
        var action = _changeHistory.Pop();
        action();
      }
    }

    public void Lock()
    {
      _isLocked = true;
    }

    public void Unlock()
    {
      _isLocked = false;
    }

    public class Guard : INotifyChangeTracker
    {
      private const string EnableKey = "changetracker_enable";

      public void NotifyCollectionWillBeCleared<T>(ITrackableCollection<T> trackableCollection)
      {
        Fail();
      }

      public void NotifyValueAdded<T>(ITrackableCollection<T> trackableCollection, T added)
      {
        Fail();
      }

      public void NotifyValueChanged<T>(ITrackableValue<T> trackableValue)
      {
        Fail();
      }

      public void NotifyValueWillBeRemoved<T>(ITrackableCollection<T> trackableCollection, T removed)
      {
        Fail();
      }

      public static void Enable()
      {
        CallContext.SetData(EnableKey, true);
      }

      public static void Disable()
      {
        CallContext.SetData(EnableKey, false);
      }

      private static void Fail()
      {
        var enable = (bool?) CallContext.GetData(EnableKey);

        if (enable != true)
          return;

        Asrt.Fail("Usage of a non initialized Trackable<> or TrackableList<> detected!");
      }
    }
  }
}