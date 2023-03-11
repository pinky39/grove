namespace Grove.Modifiers
{
  public class SwitchPowerAndToughness : Modifier, ICardModifier
  {
    private Strength _strength;

    public override void Apply(Strength strength)
    {
      _strength = strength;
      _strength.SwitchPowerAndToughness();
    }

    protected override void Unapply()
    {
      _strength.SwitchPowerAndToughness();
    }
  }
}