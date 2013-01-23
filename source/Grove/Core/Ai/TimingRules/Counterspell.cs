namespace Grove.Core.Ai.TimingRules
{
  public class Counterspell : TimingRule
  {
    public int? CounterCost;

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Stack.IsEmpty)
        return false;

      if (Stack.TopSpell.Controller == p.Controller)
        return false;

      return !CounterCost.HasValue || !p.Controller.Opponent.HasMana(CounterCost.Value);
    }
  }
}