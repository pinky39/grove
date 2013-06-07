namespace Grove.Gameplay.Modifiers
{
  using Characteristics;

  public class SetPowerAndToughness : Modifier
  {
    private readonly Value _power;
    private readonly Value _tougness;

    private Power _cardPower;
    private Toughness _cardToughness;
    private IntegerSetter _powerIntegerSetter;
    private IntegerSetter _toughnessIntegerSetter;

    private SetPowerAndToughness() {}

    public SetPowerAndToughness(Value power, Value tougness)
    {
      _power = power;
      _tougness = tougness;
    }

    public override void Apply(Power power)
    {
      _cardPower = power;
      _powerIntegerSetter = new IntegerSetter(_power.GetValue(X));
      _powerIntegerSetter.Initialize(ChangeTracker);
      _cardPower.AddModifier(_powerIntegerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessIntegerSetter = new IntegerSetter(_tougness.GetValue(X));
      _toughnessIntegerSetter.Initialize(ChangeTracker);
      _cardToughness.AddModifier(_toughnessIntegerSetter);
    }

    protected override void Unapply()
    {
      _cardPower.RemoveModifier(_powerIntegerSetter);
      _cardToughness.RemoveModifier(_toughnessIntegerSetter);
    }
  }
}