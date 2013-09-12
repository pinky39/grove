namespace Grove.Artifical.TimingRules
{
  public class WhenNoOtherInstanceOfSpellIsOnStack : TimingRule
  {
    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      return !Stack.HasSpellWithSource(p.Card);
    }
  }
}