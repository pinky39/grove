namespace Grove.Gameplay.Modifiers
{
  using System;
  using Characteristics;

  public class AddPowerAndToughness : Modifier, ICardModifier
  {
    private readonly Value _power;
    private readonly Value _toughness;
    private IntegerIncrement _powerIntegerIncrement;
    private Strenght _strenght;
    private IntegerIncrement _toughnessIntegerIncrement;

    private AddPowerAndToughness() {}

    public AddPowerAndToughness(Value power, Value toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;

      _powerIntegerIncrement = new IntegerIncrement(_power.GetValue(X));
      _powerIntegerIncrement.Initialize(ChangeTracker);
      _strenght.AddPowerModifier(_powerIntegerIncrement);

      _toughnessIntegerIncrement = new IntegerIncrement(_toughness.GetValue(X));
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