namespace Grove.Core.Modifiers
{
  public class SetPowerAndToughness : Modifier
  {
    private readonly Value _power;
    private readonly Value _tougness;
    
    private Power _cardPower;
    private Toughness _cardToughness;
    private StrenghtSetter _powerSetter;
    private StrenghtSetter _toughnessSetter;

    public SetPowerAndToughness(Value power, Value tougness)
    {
      _power = power;
      _tougness = tougness;
    }

    public override void Apply(Power power)
    {
      _cardPower = power;
      _powerSetter = new StrenghtSetter(_power.GetValue(X));
      _powerSetter.Initialize(ChangeTracker);
      _cardPower.AddModifier(_powerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessSetter = new StrenghtSetter(_tougness.GetValue(X));
      _toughnessSetter.Initialize(ChangeTracker);
      _cardToughness.AddModifier(_toughnessSetter);
    }

    protected override void Unapply()
    {
      _cardPower.RemoveModifier(_powerSetter);
      _cardToughness.RemoveModifier(_toughnessSetter);
    }
  }
}