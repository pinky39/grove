namespace Grove.Modifiers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class ChangeToCreature : Modifier, ICardModifier
  {
    private readonly List<CardColor> _colors;
    private readonly Func<Modifier, int> _power;
    private readonly Func<Modifier, int> _toughness;
    private readonly Func<Modifier, CardType> _type;
    private CardColorSetter _cardColorSetter;
    private ColorsOfCard _colorsOfCard;
    private TypeOfCard _typeOfCard;
    private IntegerSetter _powerIntegerSetter;
    private Strength _strength;
    private IntegerSetter _toughnessIntegerSetter;
    private CardTypeSetter _typeSetter;

    private ChangeToCreature() { }

    public ChangeToCreature(Func<Modifier, int> power, Func<Modifier, int> toughness, Func<Modifier, CardType> type,
      IEnumerable<CardColor> colors = null)
    {
      _power = power;
      _toughness = toughness;
      _type = type;
      _colors = colors == null ? null : colors.ToList();
    }

    public ChangeToCreature(Value power, Value toughness, Func<CardType, CardType> type, IEnumerable<CardColor> colors = null) : this(
      m => power.GetValue(m.X), m => toughness.GetValue(m.X), m => type(m.OwningCard.Type), colors)
    { }

    public override void Apply(ColorsOfCard color)
    {
      if (_colors == null)
        return;

      _colorsOfCard = color;
      _cardColorSetter = new CardColorSetter(_colors);
      _cardColorSetter.Initialize(ChangeTracker);
      _colorsOfCard.AddModifier(_cardColorSetter);
    }

    public override void Apply(Strength strength)
    {
      _strength = strength;

      _powerIntegerSetter = new IntegerSetter(_power(this));
      _powerIntegerSetter.Initialize(ChangeTracker);
      _strength.AddPowerModifier(_powerIntegerSetter);


      _toughnessIntegerSetter = new IntegerSetter(_toughness(this));
      _toughnessIntegerSetter.Initialize(ChangeTracker);
      _strength.AddToughnessModifier(_toughnessIntegerSetter);
    }

    public override void Apply(TypeOfCard typeOfCard)
    {
      _typeOfCard = typeOfCard;
      _typeSetter = new CardTypeSetter(_type(this));
      _typeSetter.Initialize(ChangeTracker);
      _typeOfCard.AddModifier(_typeSetter);
    }

    protected override void Unapply()
    {
      _strength.RemovePowerModifier(_powerIntegerSetter);
      _strength.RemoveToughnessModifier(_toughnessIntegerSetter);

      if (_cardColorSetter != null)
        _colorsOfCard.RemoveModifier(_cardColorSetter);

      _typeOfCard.RemoveModifier(_typeSetter);
    }
  }
}