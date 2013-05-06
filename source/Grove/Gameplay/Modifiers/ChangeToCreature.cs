namespace Grove.Gameplay.Modifiers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Characteristics;

  public class ChangeToCreature : Modifier
  {
    private readonly List<CardColor> _colors;
    private readonly Func<Modifier, int> _power;
    private readonly Func<Modifier, int> _toughness;
    private readonly Func<Modifier, string> _type;
    private CardColorSetter _cardColorSetter;
    private CardColors _cardColors;
    private Power _cardPower;
    private Toughness _cardToughness;
    private CardTypeCharacteristic _cardType;
    private StrenghtSetter _powerSetter;
    private StrenghtSetter _toughnessSetter;
    private CardTypeSetter _typeSetter;

    private ChangeToCreature() {}

    public ChangeToCreature(Func<Modifier, int> power, Func<Modifier, int> toughness, Func<Modifier, string> type,
      IEnumerable<CardColor> colors = null)
    {
      _power = power;
      _toughness = toughness;
      _type = type;
      _colors = colors == null ? null : colors.ToList();
    }

    public ChangeToCreature(Value power, Value toughness, string type, IEnumerable<CardColor> colors = null) : this(
      m => power.GetValue(m.X), m => toughness.GetValue(m.X), m => type, colors) {}

    public override void Apply(CardColors color)
    {
      if (_colors == null)
        return;

      _cardColors = color;
      _cardColorSetter = new CardColorSetter(_colors);
      _cardColorSetter.Initialize(ChangeTracker);
      _cardColors.AddModifier(_cardColorSetter);
    }

    public override void Apply(Power power)
    {
      _cardPower = power;
      _powerSetter = new StrenghtSetter(_power(this));
      _powerSetter.Initialize(ChangeTracker);
      _cardPower.AddModifier(_powerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessSetter = new StrenghtSetter(_toughness(this));
      _toughnessSetter.Initialize(ChangeTracker);
      _cardToughness.AddModifier(_toughnessSetter);
    }

    public override void Apply(CardTypeCharacteristic cardType)
    {
      _cardType = cardType;
      _typeSetter = new CardTypeSetter(_type(this));
      _typeSetter.Initialize(ChangeTracker);
      _cardType.AddModifier(_typeSetter);
    }

    protected override void Unapply()
    {
      _cardPower.RemoveModifier(_powerSetter);
      _cardToughness.RemoveModifier(_toughnessSetter);

      if (_cardColorSetter != null)
        _cardColors.RemoveModifier(_cardColorSetter);

      _cardType.RemoveModifier(_typeSetter);
    }
  }
}