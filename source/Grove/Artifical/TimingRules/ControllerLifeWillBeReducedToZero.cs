namespace Grove.Artifical.TimingRules
{
  using System;

  [Serializable]
  public class ControllerLifeWillBeReducedToZero : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Stack.CanTopSpellReducePlayersLifeToZero(p.Controller);
    }
  }
}