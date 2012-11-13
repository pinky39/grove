namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;
  using Mana;

  public class ColorsSetter : PropertyModifier<ManaColors>
  {
    private readonly ManaColors _value;

    private ColorsSetter() : base(null) {}

    public ColorsSetter(ManaColors value, ChangeTracker changeTracker) : base(changeTracker)
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