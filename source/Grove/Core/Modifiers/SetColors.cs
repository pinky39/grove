namespace Grove.Core.Modifiers
{
  using Mana;

  public class SetColors : Modifier
  {
    private readonly ManaColors _colors;
    private CardColors _cardColors;
    private ColorsSetter _colorsSetter;

    private SetColors() {}

    public SetColors(ManaColors colors)
    {
      _colors = colors;
    }

    public override void Apply(CardColors colors)
    {
      _cardColors = colors;
      _colorsSetter = new ColorsSetter(_colors, ChangeTracker);
      _cardColors.AddModifier(_colorsSetter);
    }

    protected override void Unapply()
    {
      _cardColors.RemoveModifier(_colorsSetter);
    }
  }
}