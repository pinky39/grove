namespace Grove.Core.Ai.TimingRules
{
  public class OwningCardWillBeDestroyed : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return CanBeDestroyed(p);
    }
  }
}