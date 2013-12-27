namespace Grove.Artifical.TimingRules
{
  public class WhenAttachedToCardWillBeDestroyed : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      if (p.Card.AttachedTo == null)
        return false;
      
      return CanBeDestroyed(p.Card.AttachedTo);
    }
  }
}