namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;

  public class StrenghtSetter : PropertyModifier<int?>
  {
    private readonly int _value;

    private StrenghtSetter() : base(null) {}

    public StrenghtSetter(int value, ChangeTracker changeTracker) : base(changeTracker)
    {
      _value = value;
    }

    public override int Priority { get { return 1; } }

    public override int? Apply(int? before)
    {
      return _value;
    }
  }
}