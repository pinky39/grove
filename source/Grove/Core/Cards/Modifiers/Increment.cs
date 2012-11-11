namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;

  public class Increment : PropertyModifier<int?>
  {
    private readonly Trackable<int> _value;

    private Increment() : base(null) {}

    public Increment(int initialValue, ChangeTracker changeTracker) : base(changeTracker)
    {
      _value = new Trackable<int>(initialValue, changeTracker);
    }

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