namespace Grove.Core.Ai.TimingRules
{
  using System;
  using System.Linq;

  public class ControllerHasMorePermanents : TimingRule
  {
    public Func<Card, bool> Selector = delegate { return true; };

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var controllerCount = p.Controller.Battlefield.Count(Selector);
      var opponentCount = p.Controller.Opponent.Battlefield.Count(Selector);

      return controllerCount > opponentCount;
    }
  }
}