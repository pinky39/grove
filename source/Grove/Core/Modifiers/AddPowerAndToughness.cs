namespace Grove.Core.Modifiers
{
  public class AddPowerAndToughness : Modifier
  {
    private Power _power;
    private Increment _powerIncrement;
    private Toughness _toughness;
    private Increment _toughnessIncrement;

    public AddPowerAndToughness()
    {
      Power = 0;
      Toughness = 0;
    }

    public Value Power { get; set; }
    public Value Toughness { get; set; }

    public override void Apply(Power power)
    {
      _power = power;
      _powerIncrement = new Increment(Power.GetValue(X), ChangeTracker);
      _power.AddModifier(_powerIncrement);
    }

    public override void Apply(Toughness toughness)
    {
      _toughness = toughness;
      _toughnessIncrement = new Increment(Toughness.GetValue(X), ChangeTracker);
      toughness.AddModifier(_toughnessIncrement);
    }

    protected override void Unapply()
    {
      _power.RemoveModifier(_powerIncrement);
      _toughness.RemoveModifier(_toughnessIncrement);
    }
  }
}