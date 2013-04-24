namespace Grove.Gameplay.Card.Counters
{
  using Characteristics;
  using Modifiers;

  public class PowerToughness : Counter
  {
    private readonly int _power;
    private readonly int _toughness;
    private Power _cardPower;
    private Toughness _cardToughness;
    private Increment _powerIncrement;
    private Increment _toughnessIncrement;

    private PowerToughness() {}

    public PowerToughness(int power, int toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    public override void ModifyPower(Power power)
    {
      _cardPower = power;

      _powerIncrement = new Increment(_power);
      _powerIncrement.Initialize(ChangeTracker);

      power.AddModifier(_powerIncrement);
    }

    public override void ModifyToughness(Toughness toughness)
    {
      _cardToughness = toughness;

      _toughnessIncrement = new Increment(_toughness);
      _toughnessIncrement.Initialize(ChangeTracker);

      toughness.AddModifier(_toughnessIncrement);
    }

    public override void Remove()
    {
      _cardPower.RemoveModifier(_powerIncrement);
      _cardToughness.RemoveModifier(_toughnessIncrement);
    }
  }
}