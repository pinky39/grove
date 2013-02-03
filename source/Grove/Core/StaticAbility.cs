namespace Grove.Core
{
  using Infrastructure;

  [Copyable]
  public class StaticAbility : IHashable
  {
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>();
    private readonly Trackable<Static> _value;

    private StaticAbility() {}

    public StaticAbility(Static value)
    {      
      _value = new Trackable<Static>(value);
    }

    public StaticAbility Initialize(ChangeTracker changeTracker)
    {
      _isEnabled.Initialize(changeTracker);
      _value.Initialize(changeTracker);

      return this;
    }

    public Static Value { get { return _value.Value; } }
    public bool IsEnabled { get { return _isEnabled.Value; } private set { _isEnabled.Value = value; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_value),
        calc.Calculate(_isEnabled)
        );
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