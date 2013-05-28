namespace Grove.Artifical.TimingRules
{
  using System;
  using Gameplay.States;

  [Serializable]
  public class DeclareBlockers : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.DeclareBlockers;
    }
  }
}