namespace Grove.Artifical.TimingRules
{
  using System;

  [Serializable]
  public class OwningCardWillBeDestroyed : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return CanBeDestroyed(p);
    }
  }
}