namespace Grove.Core.Ai.CostRules
{
  using System;

  public class OpponentProperty : CostRule
  {
    private readonly Func<Player, int> _selector;

    private OpponentProperty() {}

    public OpponentProperty(Func<Player, int> selector)
    {
      _selector = selector;
    }

    public override int CalculateX(CostRuleParameters p)
    {
      var value = _selector(p.Controller.Opponent);
      return value > p.MaxX ? p.MaxX : value;
    }
  }
}