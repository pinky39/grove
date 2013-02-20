namespace Grove.Core.Modifiers
{
  using Mana;

  public class ColorsSetter : PropertyModifier<ManaColors>
  {
    private readonly ManaColors _value;

    private ColorsSetter() {}

    public ColorsSetter(ManaColors value)
    {
      _value = value;
    }

    public override int Priority { get { return 1; } }

    public override ManaColors Apply(ManaColors before)
    {
      return _value;
    }
  }
}