namespace Grove.Core.Ai.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class Steps : TimingRule
  {
    private readonly List<Step> _steps = new List<Step>();

    public void At(params Step[] steps)
    {
      _steps.AddRange(steps);
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return _steps.Any(x => x == Turn.Step);
    }
  }
}