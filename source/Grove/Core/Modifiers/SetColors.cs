namespace Grove.Core.Modifiers
{
  using Grove.Core.Mana;

  public class SetColors : Modifier
  {
    private CardColors _cardColors;
    private ColorsSetter _colorsSetter;
    public ManaColors Colors { get; set; }


    public override void Apply(CardColors colors)
    {
      _cardColors = colors;
      _colorsSetter = new ColorsSetter(Colors, ChangeTracker);
      _cardColors.AddModifier(_colorsSetter);
    }

    protected override void Unapply()
    {
      _cardColors.RemoveModifier(_colorsSetter);
    }
  }
}