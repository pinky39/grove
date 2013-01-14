namespace Grove.Core
{
  using Infrastructure;

  [Copyable]
  public class StaticAbility : IHashable
  {
    private readonly Trackable<bool> _isEnabled;
    private readonly Trackable<Static> _value;

    private StaticAbility() {}

    public StaticAbility(Static value, ChangeTracker changeTracker)
    {
      _value = new Trackable<Static>(value, changeTracker);
      _isEnabled = new Trackable<bool>(true, changeTracker);
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