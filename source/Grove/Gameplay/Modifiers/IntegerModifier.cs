namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;

  public abstract class IntegerModifier : PropertyModifier<int?>
  {
    private readonly Trackable<int?> _value;

    protected IntegerModifier()
    {
      _value = new Trackable<int?>();
    }

    protected IntegerModifier(int? value)
    {
      _value = new Trackable<int?>(value);
    }

    public int? Value
    {
      get { return _value.Value; }
      set
      {
        _value.Value = value;
        NotifyModifierHasChanged();
      }
    }

    public override void Initialize(INotifyChangeTracker changeTracker)
    {
      base.Initialize(changeTracker);
      _value.Initialize(changeTracker);
    }
  }
}