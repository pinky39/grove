namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;

  public class MinAttackerCount : TimingRule
  {
    public int Count;
    
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Combat.Attackers.Count() >= Count;
    }
  }
}