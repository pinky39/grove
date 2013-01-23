namespace Grove.Core.Ai.TimingRules
{
  public class HasMana : TimingRule
  {
    public int Converted;

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.HasMana(Converted);
    }
  }
}