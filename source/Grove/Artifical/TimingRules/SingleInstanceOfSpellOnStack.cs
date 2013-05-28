namespace Grove.Artifical.TimingRules
{
  using System;

  [Serializable]
  public class SingleInstanceOfSpellOnStack : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return !Stack.HasSpellWithSource(p.Card);
    }
  }
}