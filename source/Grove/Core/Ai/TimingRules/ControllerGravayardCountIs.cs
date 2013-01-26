namespace Grove.Core.Ai.TimingRules
{
  using System;
  using System.Linq;

  public class ControllerGravayardCountIs : TimingRule
  {
    public int? MaxCount;
    public int? MinCount;
    public Func<Card, bool> Selector = delegate { return true; };

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var result = true;

      if (MinCount.HasValue)
        result = result && p.Controller.Graveyard.Count(Selector) >= MinCount;

      if (MaxCount.HasValue)
        result = result && p.Controller.Graveyard.Count(Selector) <= MaxCount;

      return result;
    }
  }
}