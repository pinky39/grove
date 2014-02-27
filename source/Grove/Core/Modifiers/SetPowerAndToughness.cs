namespace Grove.Modifiers
{
  public class SetPowerAndToughness : Modifier, ICardModifier
  {
    private readonly Value _power;
    private readonly Value _tougness;

    private Strenght _strenght;    
    private IntegerSetter _powerIntegerSetter;
    private IntegerSetter _toughnessIntegerSetter;

    private SetPowerAndToughness() {}

    public SetPowerAndToughness(Value power, Value tougness)
    {
      _power = power;
      _tougness = tougness;
    }

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;
      _powerIntegerSetter = new IntegerSetter(_power.GetValue(X));
      _powerIntegerSetter.Initialize(ChangeTracker);
      _strenght.AddPowerModifier(_powerIntegerSetter);

      _toughnessIntegerSetter = new IntegerSetter(_tougness.GetValue(X));
      _toughnessIntegerSetter.Initialize(ChangeTracker);
      _strenght.AddToughnessModifier(_toughnessIntegerSetter);      
    }        

    protected override void Unapply()
    {
      _strenght.RemovePowerModifier(_powerIntegerSetter);
      _strenght.RemoveToughnessModifier(_toughnessIntegerSetter);      
    }
  }
}