namespace Grove.Gameplay.Card.Abilities
{
  using Grove.Infrastructure;

  [Copyable]
  public class StaticAbility : IHashable
  {
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>(true);
    private readonly Static _value;

    private StaticAbility() {}

    public StaticAbility(Static value)
    {
      _value = value;            
    }

    public StaticAbility Initialize(INotifyChangeTracker changeTracker)
    {
      _isEnabled.Initialize(changeTracker);      

      return this;
    }

    public Static Value { get { return _value; } }
    public bool IsEnabled { get { return _isEnabled.Value; } private set { _isEnabled.Value = value; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        _value.GetHashCode(),
        _isEnabled.Value.GetHashCode());
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