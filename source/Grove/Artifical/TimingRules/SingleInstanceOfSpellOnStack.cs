namespace Grove.Artifical.TimingRules
{
  public class SingleInstanceOfSpellOnStack : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return !Stack.HasSpellWithSource(p.Card);
    }
  }
}