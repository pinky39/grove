namespace Grove.Artifical.TimingRules
{
  public class ControllerHasMana : TimingRule
  {
    private readonly int _converted;

    private ControllerHasMana() {}

    public ControllerHasMana(int converted)
    {
      _converted = converted;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.HasMana(_converted);
    }
  }
}