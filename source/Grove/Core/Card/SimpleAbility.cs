namespace Grove
{
  using Grove.Infrastructure;

  [Copyable]
  public class SimpleAbility : IHashable
  {
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>(true);
    private readonly Static _value;

    private SimpleAbility() {}

    public SimpleAbility(Static value)
    {
      _value = value;
    }

    public Static Value { get { return _value; } }
    public bool IsEnabled { get { return _isEnabled.Value; } private set { _isEnabled.Value = value; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        _value.GetHashCode(),
        _isEnabled.Value.GetHashCode());
    }

    public SimpleAbility Initialize(INotifyChangeTracker changeTracker)
    {
      _isEnabled.Initialize(changeTracker);

      return this;
    }

    public void Enable()
    {
      IsEnabled = true;
    }

    public void Disable()
    {
      IsEnabled = false;
    }
  }
}