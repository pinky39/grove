namespace Grove.Gameplay.Modifiers
{
  using Core;
  using Infrastructure;

  public class StrenghtSetter : PropertyModifier<int?>
  {
    private readonly int _value;

    private StrenghtSetter() {}

    public StrenghtSetter(int value)
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