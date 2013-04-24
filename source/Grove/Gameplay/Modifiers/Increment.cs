namespace Grove.Core.Modifiers
{
  using Infrastructure;

  public class Increment : PropertyModifier<int?>
  {
    private readonly Trackable<int> _value;

    public Increment(int value)
    {
      _value = new Trackable<int>(value);
    }

    private Increment() {}

    public int Value
    {
      get { return _value; }
      set
      {
        _value.Value = value;
        NotifyModifierHasChanged();
      }
    }

    public override int Priority { get { return 2; } }

    public override void Initialize(ChangeTracker changeTracker)
    {
      base.Initialize(changeTracker);
      _value.Initialize(changeTracker);
    }

    public override int? Apply(int? before)
    {
      return before + _value;
    }

    public static Increment operator ++(Increment increment)
    {
      increment.Value++;
      return increment;
    }

    public static Increment operator --(Increment increment)
    {
      increment.Value--;
      return increment;
    }
  }
}