namespace Grove.Modifiers
{
  using System;

  public class AddPowerAndToughness : Modifier, ICardModifier
  {
    private readonly Func<Player, Value> _power;
    private readonly Func<Player, Value> _toughness;
    private IntegerIncrement _powerIntegerIncrement;
    private Strength _strength;
    private IntegerIncrement _toughnessIntegerIncrement;

    private AddPowerAndToughness() { }

    public AddPowerAndToughness(Value power, Value toughness)
      : this(getPower: (p) => power, getToughness: (p) => toughness) { }

    public AddPowerAndToughness(Func<Player, Value> getPower, Func<Player, Value> getToughness)
    {
      _power = getPower;
      _toughness = getToughness;
    }

    public override void Apply(Strength strength)
    {
      _strength = strength;

      _powerIntegerIncrement = new IntegerIncrement(_power(OwningCard.Controller).GetValue(X));
      _powerIntegerIncrement.Initialize(ChangeTracker);
      _strength.AddPowerModifier(_powerIntegerIncrement);

      _toughnessIntegerIncrement = new IntegerIncrement(_toughness(OwningCard.Controller).GetValue(X));
      _toughnessIntegerIncrement.Initialize(ChangeTracker);
      _strength.AddToughnessModifier(_toughnessIntegerIncrement);
    }

    protected override void Unapply()
    {
      _strength.RemovePowerModifier(_powerIntegerIncrement);
      _strength.RemoveToughnessModifier(_toughnessIntegerIncrement);
    }
  }
}