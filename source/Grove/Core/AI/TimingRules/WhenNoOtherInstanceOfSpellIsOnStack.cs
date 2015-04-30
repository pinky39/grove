namespace Grove.AI.TimingRules
{
  public class WhenNoOtherInstanceOfSpellIsOnStack : TimingRule
  {
    public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
    {
      return !Stack.HasSpellWithSource(p.Card);
    }
  }
}