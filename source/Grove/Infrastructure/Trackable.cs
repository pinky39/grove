namespace Grove.Infrastructure
{
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
}