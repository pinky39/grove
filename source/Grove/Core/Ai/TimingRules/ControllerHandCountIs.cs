namespace Grove.Core.Ai.TimingRules
{
  using System;
  using System.Linq;

  public class ControllerHandCountIs : TimingRule
  {
    public Func<Card, bool> Selector = delegate { return true; };
    public int? MinCount = null;
    public int? MaxCount = null;
        
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var result = true;
      
      if (MinCount.HasValue)
        result = result && p.Controller.Hand.Count(Selector) >= MinCount;

      if (MaxCount.HasValue)
        result = result && p.Controller.Hand.Count(Selector) <= MaxCount;

      return result;
    }
  }
}