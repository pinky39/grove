namespace Grove.Ai.CostRules
{
  using System;
  using Gameplay.Player;

  public class ControllersProperty : CostRule
  {
    private readonly Func<Player, int> _selector;

    private ControllersProperty() {}

    public ControllersProperty(Func<Player, int> selector)
    {
      _selector = selector;
    }

    public override int CalculateX(CostRuleParameters p)
    {
      var value = _selector(p.Controller);
      return value > p.MaxX ? p.MaxX : value;
    }
  }
}