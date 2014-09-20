namespace Grove.Modifiers
{
  public class SwitchPowerAndToughness : Modifier, ICardModifier
  {
    private Strenght _strenght;

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;
      _strenght.SwitchPowerAndToughness();
    }
    
    protected override void Unapply()
    {
      _strenght.SwitchPowerAndToughness();
    }
  }
}