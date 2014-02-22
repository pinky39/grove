namespace Grove.Gameplay.Modifiers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class ChangeToCreature : Modifier, ICardModifier
  {
    private readonly List<CardColor> _colors;
    private readonly Func<Modifier, int> _power;
    private readonly Func<Modifier, int> _toughness;
    private readonly Func<Modifier, string> _type;
    private CardColorSetter _cardColorSetter;
    private CardColors _cardColors;    
    private CardTypeCharacteristic _cardType;
    private IntegerSetter _powerIntegerSetter;
    private IntegerSetter _toughnessIntegerSetter;
    private CardTypeSetter _typeSetter;
    private Strenght _strenght;

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

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;

      _powerIntegerSetter = new IntegerSetter(_power(this));
      _powerIntegerSetter.Initialize(ChangeTracker);
      _strenght.AddPowerModifier(_powerIntegerSetter);


      _toughnessIntegerSetter = new IntegerSetter(_toughness(this));
      _toughnessIntegerSetter.Initialize(ChangeTracker);
      _strenght.AddToughnessModifier(_toughnessIntegerSetter);
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
      _strenght.RemovePowerModifier(_powerIntegerSetter);
      _strenght.RemoveToughnessModifier(_toughnessIntegerSetter);      

      if (_cardColorSetter != null)
        _cardColors.RemoveModifier(_cardColorSetter);

      _cardType.RemoveModifier(_typeSetter);
    }
  }
}