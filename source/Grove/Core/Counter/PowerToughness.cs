namespace Grove
{
  using Modifiers;

  public class PowerToughness : Counter
  {
    private readonly int _power;
    private readonly int _toughness;
    private Strength _strength;
    private IntegerIncrement _powerIntegerIncrement;
    private IntegerIncrement _toughnessIntegerIncrement;

    private PowerToughness() { }

    public PowerToughness(int power, int toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    public override CounterType Type { get { return CounterType.PowerToughness; } }

    public override void ModifyStrength(Strength strength)
    {
      _strength = strength;

      _powerIntegerIncrement = new IntegerIncrement(_power);
      _powerIntegerIncrement.Initialize(ChangeTracker);

      _strength.AddPowerModifier(_powerIntegerIncrement);

      _toughnessIntegerIncrement = new IntegerIncrement(_toughness);
      _toughnessIntegerIncrement.Initialize(ChangeTracker);

      _strength.AddToughnessModifier(_toughnessIntegerIncrement);
    }

    public override void Remove()
    {
      _strength.RemovePowerModifier(_powerIntegerIncrement);
      _strength.RemoveToughnessModifier(_toughnessIntegerIncrement);
    }
  }
}