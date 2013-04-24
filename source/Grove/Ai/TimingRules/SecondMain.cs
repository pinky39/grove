﻿namespace Grove.Ai.TimingRules
{
  using Core;
  using Gameplay.States;

  public class SecondMain : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.SecondMain;
    }
  }
}