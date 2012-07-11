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
    private readonly ChangeTracker _changeTracker;
    private readonly IHashDependancy _hashDependency;
    private T _value;

    public Trackable(ChangeTracker changeTracker, IHashDependancy hashDependancy = null)
      : this(default(T), changeTracker, hashDependancy) {}

    public Trackable(T value, ChangeTracker changeTracker, IHashDependancy hashDependancy = null)
    {
      _value = value;
      _changeTracker = changeTracker;
      _hashDependency = hashDependancy ?? new NullHashDependency();
    }

    private Trackable() {}

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