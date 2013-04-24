namespace Grove.Ai.TimingRules
{
  public class OwningCardWillBeDestroyed : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return CanBeDestroyed(p);
    }
  }
}