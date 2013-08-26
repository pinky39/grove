namespace Grove.Artifical.TimingRules
{
  using System;
  using Gameplay.States;

  public class OnYourTurn : TimingRule
  {
    private readonly Step _step;

    private OnYourTurn()
    {      
    }

    public OnYourTurn(Step step)
    {
      _step = step;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.IsActive && Turn.Step == _step;
    }
  }
}