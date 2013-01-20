namespace Grove.Core.Ai.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions.Results;
  using Mana;

  public class Steps : TimingRule
  {
    private List<Step> _steps = new List<Step>();

    public void At(params Step[] steps)
    {
      _steps.AddRange(steps);
    }

    public override bool ShouldPlay(Playable playable, ActivationPrerequisites prerequisites)
    {
      return _steps.Any(x => x == Turn.Step);
    }
  }
}