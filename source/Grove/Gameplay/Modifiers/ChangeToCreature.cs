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
    private IntegerSetter _powerIntegerSetter;
    private IntegerSetter _toughnessIntegerSetter;
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
      _powerIntegerSetter = new IntegerSetter(_power(this));
      _powerIntegerSetter.Initialize(ChangeTracker);
      _cardPower.AddModifier(_powerIntegerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessIntegerSetter = new IntegerSetter(_toughness(this));
      _toughnessIntegerSetter.Initialize(ChangeTracker);
      _cardToughness.AddModifier(_toughnessIntegerSetter);
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
      _cardPower.RemoveModifier(_powerIntegerSetter);
      _cardToughness.RemoveModifier(_toughnessIntegerSetter);

      if (_cardColorSetter != null)
        _cardColors.RemoveModifier(_cardColorSetter);

      _cardType.RemoveModifier(_typeSetter);
    }
  }
}