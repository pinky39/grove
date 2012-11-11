namespace Grove.Core.Details.Cards.Modifiers
{
  public class SetPowerAndToughness : Modifier
  {
    private Power _cardPower;
    private Toughness _cardToughness;
    private StrenghtSetter _powerSetter;
    private StrenghtSetter _toughnessSetter;

    public Value Power { get; set; }
    public Value Tougness { get; set; }

    public override void Apply(Power power)
    {
      _cardPower = power;
      _powerSetter = new StrenghtSetter(Power.GetValue(X), ChangeTracker);
      _cardPower.AddModifier(_powerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessSetter = new StrenghtSetter(Tougness.GetValue(X), ChangeTracker);
      _cardToughness.AddModifier(_toughnessSetter);
    }

    protected override void Unapply()
    {
      _cardPower.RemoveModifier(_powerSetter);
      _cardToughness.RemoveModifier(_toughnessSetter);
    }
  }
}