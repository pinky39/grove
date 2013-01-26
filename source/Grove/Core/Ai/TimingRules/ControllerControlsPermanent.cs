namespace Grove.Core.Ai.TimingRules
{
  using System;
  using System.Linq;

  public class ControllerControlsPermanent : TimingRule
  {
    public Func<Card, bool> Selector = delegate { return true; };
    public int MinCount = 1;
    
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Players.Permanents().Count(x => Selector(x) && x.Controller == p.Controller) >= MinCount;
    }
  }
}