namespace Grove.AI.TimingRules
{
  public class WhenAttachedToCardWillBeDestroyed : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      if (p.Card.AttachedTo == null)
        return false;
      
      return CanBeDestroyed(p.Card.AttachedTo);
    }
  }
}