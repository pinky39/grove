namespace Grove.Modifiers
{
  public class SetPowerAndToughness : Modifier, ICardModifier
  {
    private readonly Value _power;
    private readonly Value _tougness;

    private Strength _strength;
    private IntegerSetter _powerIntegerSetter;
    private IntegerSetter _toughnessIntegerSetter;

    private SetPowerAndToughness() { }

    public SetPowerAndToughness(Value power, Value tougness)
    {
      _power = power;
      _tougness = tougness;
    }

    public override void Apply(Strength strength)
    {
      _strength = strength;
      _powerIntegerSetter = new IntegerSetter(_power.GetValue(X));
      _powerIntegerSetter.Initialize(ChangeTracker);
      _strength.AddPowerModifier(_powerIntegerSetter);

      _toughnessIntegerSetter = new IntegerSetter(_tougness.GetValue(X));
      _toughnessIntegerSetter.Initialize(ChangeTracker);
      _strength.AddToughnessModifier(_toughnessIntegerSetter);
    }

    protected override void Unapply()
    {
      _strength.RemovePowerModifier(_powerIntegerSetter);
      _strength.RemoveToughnessModifier(_toughnessIntegerSetter);
    }
  }
}