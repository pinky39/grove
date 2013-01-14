namespace Grove.Core.Counters
{
  using Modifiers;

  public class PowerToughness : Counter
  {
    private Power _power;
    private Increment _powerIncrement;
    private Toughness _toughness;
    private Increment _toughnessIncrement;

    public int Power { get; set; }
    public int Toughness { get; set; }

    public override void ModifyPower(Power power)
    {
      _power = power;
      _powerIncrement = new Increment(Power, ChangeTracker);
      power.AddModifier(_powerIncrement);
    }

    public override void ModifyToughness(Toughness toughness)
    {
      _toughness = toughness;
      _toughnessIncrement = new Increment(Toughness, ChangeTracker);
      toughness.AddModifier(_toughnessIncrement);
    }

    public override void Remove()
    {
      _power.RemoveModifier(_powerIncrement);
      _toughness.RemoveModifier(_toughnessIncrement);
    }
  }
}