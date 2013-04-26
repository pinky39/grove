namespace Grove.Ai.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.States;

  public class Steps : TimingRule
  {
    private readonly bool _activeTurn;
    private readonly bool _passiveTurn;

    private readonly List<Step> _steps = new List<Step>();

    private Steps() {}
    public Steps(params Step[] steps) : this(true, true, steps) {}

    public Steps(bool activeTurn, bool passiveTurn, params Step[] steps)
    {
      _activeTurn = activeTurn;
      _passiveTurn = passiveTurn;
      _steps.AddRange(steps);
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if ((p.Controller.IsActive && _activeTurn) || (!p.Controller.IsActive && _passiveTurn))
      {
        return _steps.Any(x => x == Turn.Step);
      }

      return false;
    }
  }
}