namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;

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