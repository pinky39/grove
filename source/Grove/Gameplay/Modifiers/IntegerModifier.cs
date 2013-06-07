namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;

  public abstract class IntegerModifier : PropertyModifier<int?>
  {
    private readonly Trackable<int?> _value = new Trackable<int?>();

    protected IntegerModifier() {}    

    protected IntegerModifier(int? value)
    {
      _value.Value = value;
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