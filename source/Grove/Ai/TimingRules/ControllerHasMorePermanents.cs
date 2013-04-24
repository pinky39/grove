namespace Grove.Ai.TimingRules
{
  using System;
  using System.Linq;
  using Core;
  using Gameplay.Card;

  public class ControllerHasMorePermanents : TimingRule
  {
    private readonly Func<Card, bool> _selector;

    private ControllerHasMorePermanents() {}

    public ControllerHasMorePermanents(Func<Card, bool> selector = null)
    {
      _selector = selector ?? delegate { return true; };
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var controllerCount = p.Controller.Battlefield.Count(_selector);
      var opponentCount = p.Controller.Opponent.Battlefield.Count(_selector);

      return controllerCount > opponentCount;
    }
  }
}