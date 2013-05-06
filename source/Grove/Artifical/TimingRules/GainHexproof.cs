namespace Grove.Artifical.TimingRules
{
  public class GainHexproof : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return CanBeDestroyed(p, targetOnly: true, considerCombat: false);
    }
  }
}