namespace Grove.Gameplay.Counters
{
  using Characteristics;
  using Modifiers;

  public class PowerToughness : Counter
  {
    private readonly int _power;
    private readonly int _toughness;
    private Power _cardPower;
    private Toughness _cardToughness;
    private IntegerIncrement _powerIntegerIncrement;
    private IntegerIncrement _toughnessIntegerIncrement;

    private PowerToughness() {}

    public PowerToughness(int power, int toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    public override CounterType Type { get { return CounterType.PowerToughnes; } }

    public override void ModifyPower(Power power)
    {
      _cardPower = power;

      _powerIntegerIncrement = new IntegerIncrement(_power);
      _powerIntegerIncrement.Initialize(ChangeTracker);

      power.AddModifier(_powerIntegerIncrement);
    }

    public override void ModifyToughness(Toughness toughness)
    {
      _cardToughness = toughness;

      _toughnessIntegerIncrement = new IntegerIncrement(_toughness);
      _toughnessIntegerIncrement.Initialize(ChangeTracker);

      toughness.AddModifier(_toughnessIntegerIncrement);
    }

    public override void Remove()
    {
      _cardPower.RemoveModifier(_powerIntegerIncrement);
      _cardToughness.RemoveModifier(_toughnessIntegerIncrement);
    }
  }
}