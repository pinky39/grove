namespace Grove.Artifical.TimingRules
{
  using System;
  using System.Linq;

  [Serializable]
  public class MinAttackerCount : TimingRule
  {
    private readonly int _count;

    private MinAttackerCount() {}

    public MinAttackerCount(int count)
    {
      _count = count;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Combat.Attackers.Count() >= _count;
    }
  }
}