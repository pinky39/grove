namespace Grove.Core.Modifiers
{
  public class AddPowerAndToughness : Modifier
  {
    private readonly Value _power;
    private readonly Value _toughness;
    private Power _cardPower;
    private Toughness _cardToughness;
    private Increment _powerIncrement;
    private Increment _toughnessIncrement;

    private AddPowerAndToughness() {}

    public AddPowerAndToughness(Value power, Value toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    public override void Apply(Power power)
    {
      _cardPower = power;
      _powerIncrement = new Increment(_power.GetValue(X));
      _powerIncrement.Initialize(ChangeTracker);
      _cardPower.AddModifier(_powerIncrement);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessIncrement = new Increment(_toughness.GetValue(X));
      _toughnessIncrement.Initialize(ChangeTracker);
      toughness.AddModifier(_toughnessIncrement);
    }

    protected override void Unapply()
    {
      _cardPower.RemoveModifier(_powerIncrement);
      _cardToughness.RemoveModifier(_toughnessIncrement);
    }
  }
}