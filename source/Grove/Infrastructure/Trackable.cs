namespace Grove.Infrastructure
{
  public interface IHashDependancy
  {
    void InvalidateHash();
  }

  public class NullHashDependency : IHashDependancy
  {
    public void InvalidateHash() {}
  }

  [Copyable]
  public class Trackable<T> : ITrackableValue<T>
  {
    private ChangeTracker _changeTracker;
    private IHashDependancy _hashDependency;
    private T _value;

    public Trackable() : this(default(T)) {}

    public Trackable(T value)
    {
      _value = value;
    }

    public T Value
    {
      get { return _value; }
      set
      {
        _changeTracker.NotifyValueChanged(this);
        _hashDependency.InvalidateHash();
        _value = value;
      }
    }

    T ITrackableValue<T>.Value { get { return _value; } set { _value = value; } }

    public int CalculateHash(HashCalculator calc)
    {
      var hashable = _value as IHashable;

      return hashable != null
        ? calc.Calculate(hashable)
        : _value.GetHashCode();
    }

    public void Initialize(ChangeTracker changeTracker, IHashDependancy hashDependancy = null)
    {
      _changeTracker = changeTracker;
      _hashDependency = hashDependancy ?? new NullHashDependency();
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