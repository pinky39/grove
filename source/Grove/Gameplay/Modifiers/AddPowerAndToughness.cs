namespace Grove.Gameplay.Modifiers
{
  using System;
  using Characteristics;
    
  public class AddPowerAndToughness : Modifier, ICardModifier
  {
    private readonly Value _power;
    private readonly Value _toughness;
    private Power _cardPower;
    private Toughness _cardToughness;
    private IntegerIncrement _powerIntegerIncrement;
    private IntegerIncrement _toughnessIntegerIncrement;

    private AddPowerAndToughness() {}

    public AddPowerAndToughness(Value power, Value toughness)
    {
      _power = power;
      _toughness = toughness;
    }    

    public override void Apply(Power power)
    {
      _cardPower = power;
      _powerIntegerIncrement = new IntegerIncrement(_power.GetValue(X));
      _powerIntegerIncrement.Initialize(ChangeTracker);
      _cardPower.AddModifier(_powerIntegerIncrement);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessIntegerIncrement = new IntegerIncrement(_toughness.GetValue(X));
      _toughnessIntegerIncrement.Initialize(ChangeTracker);
      toughness.AddModifier(_toughnessIntegerIncrement);
    }

    protected override void Unapply()
    {
      _cardPower.RemoveModifier(_powerIntegerIncrement);
      _cardToughness.RemoveModifier(_toughnessIntegerIncrement);
    }
  }
}