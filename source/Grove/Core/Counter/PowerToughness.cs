namespace Grove
{
  using Modifiers;

  public class PowerToughness : Counter
  {
    private readonly int _power;
    private readonly int _toughness;
    private Strenght _strenght;
    private IntegerIncrement _powerIntegerIncrement;
    private IntegerIncrement _toughnessIntegerIncrement;

    private PowerToughness() {}

    public PowerToughness(int power, int toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    public override CounterType Type { get { return CounterType.PowerToughnes; } }

    public override void ModifyStrenght(Strenght strenght)
    {
      _strenght = strenght;

      _powerIntegerIncrement = new IntegerIncrement(_power);
      _powerIntegerIncrement.Initialize(ChangeTracker);

      _strenght.AddPowerModifier(_powerIntegerIncrement);

      _toughnessIntegerIncrement = new IntegerIncrement(_toughness);
      _toughnessIntegerIncrement.Initialize(ChangeTracker);

      _strenght.AddToughnessModifier(_toughnessIntegerIncrement);            
    }        

    public override void Remove()
    {
      _strenght.RemovePowerModifier(_powerIntegerIncrement);
      _strenght.RemoveToughnessModifier(_toughnessIntegerIncrement);      
    }
  }
}