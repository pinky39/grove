namespace Grove.Core.Modifiers
{
  using System;
  using Mana;

  public class ChangeToCreature : Modifier
  {
    private readonly ManaColors? _colors;
    private readonly Func<Modifier, int> _power;
    private readonly Func<Modifier, int> _toughness;
    private readonly Func<Modifier, string> _type;
    private CardColors _cardColors;
    private Power _cardPower;
    private Toughness _cardToughness;
    private CardTypeCharacteristic _cardType;
    private ColorsSetter _colorsSetter;
    private StrenghtSetter _powerSetter;
    private StrenghtSetter _toughnessSetter;
    private CardTypeSetter _typeSetter;

    public ChangeToCreature(Func<Modifier, int> power, Func<Modifier, int> toughness, Func<Modifier, string> type,
      ManaColors? colors = null)
    {
      _power = power;
      _toughness = toughness;
      _type = type;
      _colors = colors;
    }

    public ChangeToCreature(Value power, Value toughness, string type, ManaColors? colors = null) : this(
      m => power.GetValue(m.X), m => toughness.GetValue(m.X), m => type, colors) {}

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
      _powerSetter = new StrenghtSetter(_power(this), ChangeTracker);
      _cardPower.AddModifier(_powerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessSetter = new StrenghtSetter(_toughness(this), ChangeTracker);
      _cardToughness.AddModifier(_toughnessSetter);
    }

    public override void Apply(CardTypeCharacteristic cardType)
    {
      _cardType = cardType;
      _typeSetter = new CardTypeSetter(_type(this), ChangeTracker);
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