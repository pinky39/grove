namespace Grove.Core.Modifiers
{
  using Mana;

  public class ChangeToCreature : Modifier
  {
    private CardColors _cardColors;
    private Power _cardPower;
    private Toughness _cardToughness;
    private CardTypeCharacteristic _cardType;
    private ColorsSetter _colorsSetter;
    private StrenghtSetter _powerSetter;
    private StrenghtSetter _toughnessSetter;
    private CardTypeSetter _typeSetter;

    private readonly ManaColors? _colors;
    private readonly Value _power;
    private readonly Value _toughness;
    private readonly string _type;

    public ChangeToCreature(Value power, Value toughness, string type, ManaColors? colors = null)
    {
      _colors = colors;
      _type = type;
      _toughness = toughness;
      _power = power;
    }

    public override void Apply(CardColors colors)
    {
      if (_colors == null)
        return;

      _cardColors = colors;
      _colorsSetter = new ColorsSetter(_colors.Value, ChangeTracker);
      _cardColors.AddModifier(_colorsSetter);
    }

    public override void Apply(Power power)
    {
      _cardPower = power;
      _powerSetter = new StrenghtSetter(_power.GetValue(X), ChangeTracker);
      _cardPower.AddModifier(_powerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessSetter = new StrenghtSetter(_toughness.GetValue(X), ChangeTracker);
      _cardToughness.AddModifier(_toughnessSetter);
    }

    public override void Apply(CardTypeCharacteristic cardType)
    {
      _cardType = cardType;
      _typeSetter = new CardTypeSetter(_type, ChangeTracker);
      _cardType.AddModifier(_typeSetter);
    }

    protected override void Unapply()
    {
      _cardPower.RemoveModifier(_powerSetter);
      _cardToughness.RemoveModifier(_toughnessSetter);

      if (_colorsSetter != null)
        _cardColors.RemoveModifier(_colorsSetter);

      _cardType.RemoveModifier(_typeSetter);
    }
  }
}