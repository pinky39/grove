namespace Grove.Core.Ai.TimingRules
{
  public class Counterspell : TimingRule
  {
    private int? _counterCost;

    public Counterspell(int? counterCost = null)
    {
      _counterCost = counterCost;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Stack.IsEmpty)
        return false;

      if (Stack.TopSpell.Controller == p.Controller)
        return false;

      return !_counterCost.HasValue || !p.Controller.Opponent.HasMana(_counterCost.Value);
    }
  }
}