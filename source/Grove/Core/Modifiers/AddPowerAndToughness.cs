namespace Grove.Modifiers
{
  using System;

  public class AddPowerAndToughness : Modifier, ICardModifier
  {
    private readonly Func<Player, Value> _power;
    private readonly Func<Player, Value> _toughness;
    private IntegerIncrement _powerIntegerIncrement;
    private Strenght _strenght;
    private IntegerIncrement _toughnessIntegerIncrement;

    private AddPowerAndToughness() { }

    public AddPowerAndToughness(Value power, Value toughness)
      : this(getPower: (p) => power, getToughness: (p) => toughness)
    {
    }

    public AddPowerAndToughness(Func<Player, Value> getPower, Func<Player, Value> getToughness)
    {
      _power = getPower;
      _toughness = getToughness;
    }

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;

      _powerIntegerIncrement = new IntegerIncrement(_power(OwningCard.Controller).GetValue(X));
      _powerIntegerIncrement.Initialize(ChangeTracker);
      _strenght.AddPowerModifier(_powerIntegerIncrement);

      _toughnessIntegerIncrement = new IntegerIncrement(_toughness(OwningCard.Controller).GetValue(X));
      _toughnessIntegerIncrement.Initialize(ChangeTracker);
      _strenght.AddToughnessModifier(_toughnessIntegerIncrement);
    }

    protected override void Unapply()
    {
      _strenght.RemovePowerModifier(_powerIntegerIncrement);
      _strenght.RemoveToughnessModifier(_toughnessIntegerIncrement);
    }
  }
}