namespace Grove.Artifical.TimingRules
{
  using System;
  using Gameplay.States;

  [Serializable]
  public class Cycling : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return (Turn.Step == Step.EndOfTurn && !p.Controller.IsActive && Stack.IsEmpty);
    }
  }
}