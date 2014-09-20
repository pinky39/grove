namespace Grove.AI.TimingRules
{
  using System;
  using System.Linq;

  public class WhenYouHaveMorePermanents : TimingRule
  {
    private readonly Func<Card, bool> _selector;

    private WhenYouHaveMorePermanents() {}

    public WhenYouHaveMorePermanents(Func<Card, bool> selector = null)
    {
      _selector = selector ?? delegate { return true; };
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      var controllerCount = p.Controller.Battlefield.Count(_selector);
      var opponentCount = p.Controller.Opponent.Battlefield.Count(_selector);

      return controllerCount > opponentCount;
    }
  }
}